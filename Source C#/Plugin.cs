using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using ScheduleOne.Effects;
using TMPro;
using UnityEngine;
using ScheduleOne.UI.Stations;
using UnityEngine.UI;

namespace Schedule_Mod
{
    [BepInPlugin("com.jow.profitcalc", "Profit Calculator", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(SlotChangedPatch));
            Harmony.CreateAndPatchAll(typeof(MixingStationCanvas_OpenPatch));
            Harmony.CreateAndPatchAll(typeof(MixingStationCanvas_ClosePatch));
            Log.LogInfo("Profit Calculator loaded!");
        }
    }

    [HarmonyPatch(typeof(MixingStation), "SetStoredInstance_Internal")]
    public static class SlotChangedPatch
    {
        static void Postfix(MixingStation __instance)
        {
            var productRaw = __instance.GetProduct();
            if (productRaw == null) return;

            var product = productRaw.TryCast<ProductDefinition>();
            if (product == null) return;

            var ingredients = __instance.GetIngredients();
            if (ingredients == null || ingredients.Count == 0) return;

            float totalCost = 0f;
            int ingredientCount = 0;
            var simulatedEffects = product.Properties;

            for (int i = 0; i < ingredients.Count; i++)
            {
                var item = ingredients[i];
                if (item == null || item.Definition == null) continue;

                if (item.Definition.TryCast<ProductDefinition>() != null) continue;

                var defStorable = item.Definition.TryCast<StorableItemDefinition>();
                if (defStorable == null) continue;

                float cost = defStorable.BasePurchasePrice * item.Quantity;
                totalCost += cost;
                ingredientCount++;

                var defProp = defStorable.TryCast<PropertyItemDefinition>();
                if (defProp?.Properties != null && defProp.Properties.Count > 0)
                {
                    simulatedEffects = EffectMixCalculator.MixProperties(
                        simulatedEffects,
                        defProp.Properties[0],
                        product.DrugType);
                }
            }

            int simulatedPrice = 0;
            string effectList = "";

            if (simulatedEffects != null)
            {
                float multiplierSum = 0f;
                for (int i = 0; i < simulatedEffects.Count; i++)
                {
                    multiplierSum += simulatedEffects[i].AddBaseValueMultiple;
                    effectList += string.Format("\n  - {0}", simulatedEffects[i].Name);
                }

                simulatedPrice = (int)Math.Round(product.BasePrice * (1 + multiplierSum));
            }

            int quantity = 0;
            var productSlot = __instance.ProductSlot;
            if (productSlot != null && productSlot.Quantity > 0)
                quantity = productSlot.Quantity;

            if (ProfitUI.ProfitText != null)
            {
                string effects = effectList.Replace("\n  - ", ", ").TrimStart(',').Trim();

                ProfitUI.ProfitText.text = string.Format(
                    "<color=#00FF00>=== PROFIT CALCULATOR ===</color>\n\n" +
                    "<line-height=130%>" +
                    "Product: {0}\n" +
                    "Total cost: ${1}\n" +
                    "Sell price: ${2}\n" +
                    "Effects: {3}\n" +
                    "Quantity: {4}\n" +
                    "Gross revenue: ${5}\n" +
                    "</line-height>\n" +
                    "<color=#00FF00>NET PROFIT: ${6}</color>",
                    product.name,
                    totalCost,
                    simulatedPrice,
                    effects,
                    quantity,
                    simulatedPrice * quantity,
                    (simulatedPrice * quantity) - totalCost);
            }
        }
    }

    [HarmonyPatch(typeof(MixingStationCanvas), "Open")]
    public static class MixingStationCanvas_OpenPatch
    {
        static void Postfix(MixingStationCanvas __instance)
        {
            var log = BepInEx.Logging.Logger.CreateLogSource("ProfitCalc");
            if (__instance == null) { log.LogError("__instance e null"); return; }

            var canvas = __instance.Canvas;
            if (canvas == null) { log.LogError("Canvas e null"); return; }

            ProfitUI.Panel = new GameObject("ProfitCalculatorPanel");
            ProfitUI.Panel.transform.SetParent(canvas.transform, false);

            var rect = ProfitUI.Panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.pivot = new Vector2(1f, 0.5f);
            rect.anchoredPosition = new Vector2(-310f, 0f);
            rect.sizeDelta = new Vector2(280f, 250f);

            var img = ProfitUI.Panel.AddComponent<Image>();
            var slotUI = __instance.ProductSlotUI;
            if (slotUI != null && slotUI.Background != null)
            {
                img.sprite = slotUI.Background.sprite;
                img.color = slotUI.Background.color;
                img.type = slotUI.Background.type;
                img.pixelsPerUnitMultiplier = slotUI.Background.pixelsPerUnitMultiplier;
            }
            else
            {
                img.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);
            }

            var textObj = new GameObject("ProfitText");
            textObj.transform.SetParent(ProfitUI.Panel.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10f, 10f);
            textRect.offsetMax = new Vector2(-10f, -25f);

            ProfitUI.ProfitText = textObj.AddComponent<TextMeshProUGUI>();
            ProfitUI.ProfitText.fontSize = 14f;
            ProfitUI.ProfitText.alignment = TextAlignmentOptions.Center;
            ProfitUI.ProfitText.color = Color.white;
            ProfitUI.ProfitText.text = "Waiting for ingredients...";
        }
    }

    [HarmonyPatch(typeof(MixingStationCanvas), "Close")]
    public static class MixingStationCanvas_ClosePatch
    {
        static void Postfix()
        {
            if (ProfitUI.Panel != null)
            {
                GameObject.Destroy(ProfitUI.Panel);
                ProfitUI.Panel = default;
                ProfitUI.ProfitText = default;
            }
        }
    }

    public static class ProfitUI
    {
        public static GameObject? Panel;
        public static TextMeshProUGUI? ProfitText;
    }
}