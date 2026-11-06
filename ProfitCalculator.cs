using System;
using System.Linq;
using HarmonyLib;
using MelonLoader;
using ScheduleOne.DevUtilities;
using ScheduleOne.Effects;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using ScheduleOne.UI.Stations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(ProfitCalculator.ProfitCalculatorMod), "Profit Calculator", "2.0.1", "jow")]
[assembly: MelonGame(null, null)]
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.MONO)]

namespace ProfitCalculator
{
    public class ProfitCalculatorMod : MelonMod
    {
        public override void OnInitializeMelon() => LoggerInstance.Msg("Profit Calculator 2.0.1 loaded");
    }

    // A compact card next to the mixing screen: result product and its effects, the numbers, and the net profit.
    // The card grows with its content (Unity layout), so long effect lists wrap instead of overflowing.
    [HarmonyPatch(typeof(MixingStationInterface))]
    static class Panel
    {
        static readonly Color Bg = new Color32(18, 20, 26, 242), Edge = new Color32(255, 255, 255, 26),
            Divider = new Color32(255, 255, 255, 18), GoodBg = new Color32(74, 222, 128, 30), BadBg = new Color32(248, 113, 113, 30);
        const string Muted = "#8B93A1", Text = "#E8ECF2", Good = "#4ADE80", Bad = "#F87171";

        static Sprite round;
        static GameObject panel, details;
        static TextMeshProUGUI name, effects, mixer, qty, price, cost, gain, profit;
        static Image footer;

        // white rounded square, 9-sliced so corners stay crisp at any size
        static Sprite Round()
        {
            if (round != null) return round;
            const int n = 48, r = 12;
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = Mathf.Max(r - x - 0.5f, x + 0.5f - (n - r), 0f), dy = Mathf.Max(r - y - 0.5f, y + 0.5f - (n - r), 0f);
                    tex.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(r - Mathf.Sqrt(dx * dx + dy * dy) + 0.5f)));
                }
            tex.Apply();
            return round = Sprite.Create(tex, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(r, r, r, r));
        }

        static Image Box(GameObject go, Color c, float radiusScale = 1f)
        {
            var i = go.AddComponent<Image>();
            i.sprite = Round(); i.type = Image.Type.Sliced; i.pixelsPerUnitMultiplier = radiusScale; i.color = c; i.raycastTarget = false;
            return i;
        }

        static GameObject Child(GameObject parent, string n, float height = -1)
        {
            var go = new GameObject(n, typeof(RectTransform));
            go.transform.SetParent(parent.transform, false);
            if (height >= 0) go.AddComponent<LayoutElement>().preferredHeight = height;
            return go;
        }

        static TextMeshProUGUI Label(GameObject parent, float size, TextAlignmentOptions align, bool bold = false, bool wrap = false)
        {
            var t = Child(parent, "Text").AddComponent<TextMeshProUGUI>();
            t.fontSize = size; t.alignment = align; t.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
            t.enableWordWrapping = wrap; t.overflowMode = wrap ? TextOverflowModes.Overflow : TextOverflowModes.Ellipsis;
            t.color = Color.white; t.richText = true; t.raycastTarget = false;
            return t;
        }

        static void Fill(Component c, float left = 0, float right = 0)
        {
            var r = (RectTransform)c.transform;
            r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(left, 0); r.offsetMax = new Vector2(-right, 0);
        }

        // one "label ........ value" line
        static TextMeshProUGUI Row(GameObject parent, string label)
        {
            var row = Child(parent, label, 21);
            var l = Label(row, 14, TextAlignmentOptions.MidlineLeft); Fill(l, 0, 0); l.text = $"<color={Muted}>{label}</color>";
            var v = Label(row, 14, TextAlignmentOptions.MidlineRight, true); Fill(v, 84, 0);
            return v;
        }

        [HarmonyPostfix, HarmonyPatch(nameof(MixingStationInterface.Open))]
        static void Open(MixingStationInterface __instance)
        {
            try
            {
                if (panel != null) UnityEngine.Object.Destroy(panel);
                var parent = Traverse.Create(__instance).Field("_container").GetValue<RectTransform>();

                // outer box = 1px border, inner box = card body
                panel = new GameObject("ProfitCalculatorPanel", typeof(RectTransform));
                panel.transform.SetParent(parent, false);
                var rect = (RectTransform)panel.transform;
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(1f, 0.5f);
                rect.anchoredPosition = new Vector2(-150f, 0f);
                rect.sizeDelta = new Vector2(300f, 0f);
                Box(panel, Edge);
                var fit = panel.AddComponent<ContentSizeFitter>(); fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var outer = panel.AddComponent<VerticalLayoutGroup>();
                outer.padding = new RectOffset(1, 1, 1, 1); outer.childControlWidth = outer.childControlHeight = true; outer.childForceExpandHeight = false;

                var card = Child(panel, "Card");
                Box(card, Bg);
                var v = card.AddComponent<VerticalLayoutGroup>();
                v.padding = new RectOffset(16, 16, 14, 14); v.spacing = 6;
                v.childControlWidth = v.childControlHeight = true; v.childForceExpandHeight = v.childForceExpandWidth = false;

                Label(card, 12, TextAlignmentOptions.Left, true).text = $"<color={Good}>•</color> <color={Muted}><cspace=0.12em>PROFIT CALCULATOR</cspace></color>";
                name = Label(card, 19, TextAlignmentOptions.Left, true, true);
                effects = Label(card, 13, TextAlignmentOptions.Left, false, true);
                effects.lineSpacing = 8;

                details = Child(card, "Details");
                var dv = details.AddComponent<VerticalLayoutGroup>();
                dv.spacing = 6; dv.childControlWidth = dv.childControlHeight = true; dv.childForceExpandHeight = false;
                Box(Child(details, "Divider", 1), Divider).type = Image.Type.Simple;
                mixer = Row(details, "Mixer");
                qty = Row(details, "Quantity");
                price = Row(details, "Price per unit");
                cost = Row(details, "Mixer cost");
                gain = Row(details, "Value gained");

                var foot = Child(details, "Footer", 44);
                footer = Box(foot, GoodBg, 1.5f);
                var fl = Label(foot, 14, TextAlignmentOptions.MidlineLeft, true); Fill(fl, 12, 12); fl.text = $"<color={Text}>Net profit</color>";
                profit = Label(foot, 22, TextAlignmentOptions.MidlineRight, true); Fill(profit, 90, 12);

                Update(__instance.Station);
            }
            catch (Exception e) { MelonLogger.Error("Profit Calculator: " + e); }
        }

        [HarmonyPostfix, HarmonyPatch("UpdatePreview")]
        static void Changed(MixingStationInterface __instance) => Update(__instance.Station);

        [HarmonyPostfix, HarmonyPatch(nameof(MixingStationInterface.Close))]
        static void Close()
        {
            if (panel != null) UnityEngine.Object.Destroy(panel);
            panel = details = null;
        }

        static string Money(float v) => (v < 0 ? "-$" : "$") + Math.Abs(v).ToString("N0");
        static string Signed(float v) => (v > 0 ? "+" : "") + Money(v);

        static void Update(MixingStation st)
        {
            if (panel == null || st == null) return;
            try
            {
                var product = st.GetProduct();
                var mix = st.GetMixer();
                bool ready = product != null && mix != null;
                details.SetActive(ready);
                if (!ready)
                {
                    name.text = $"<color={Text}>Add a product and a mixer</color>";
                    effects.text = $"<color={Muted}>The estimate shows up here before you mix.</color>";
                    return;
                }

                var mixed = EffectMixCalculator.MixProperties(product.Properties, mix.Properties[0], product.DrugType);
                // a known recipe yields a different product (own base price and name)
                var recipe = NetworkSingleton<ProductManager>.Instance.GetRecipe(product.Properties, mix.Properties[0]);
                var result = recipe?.Product.Item as ProductDefinition ?? product;
                int after = (int)Math.Round(ProductManager.CalculateProductValue(result.BasePrice, mixed));
                int before = (int)Math.Round(ProductManager.CalculateProductValue(product.BasePrice, product.Properties));
                int n = st.GetMixQuantity();
                float spent = ((StorableItemDefinition)mix).BasePurchasePrice * n;
                float gained = (after - before) * n;
                float net = gained - spent;

                name.text = $"<color={Text}>{result.Name}</color>";
                effects.text = string.Join("    ", mixed.Select(e =>
                {
                    var c = ColorUtility.ToHtmlStringRGB(e.LabelColor);
                    return $"<nobr><color=#{c}>• {e.Name}</color></nobr>";
                }));
                mixer.text = $"<color={Text}>{mix.Name}</color>";
                qty.text = $"<color={Text}>{n}</color>";
                price.text = $"<color={Text}>{Money(after)}</color> <size=85%><color={Muted}>(was {Money(before)})</color></size>";
                cost.text = $"<color={Text}>{Money(-spent)}</color>";
                gain.text = $"<color={(gained >= 0 ? Good : Bad)}>{Signed(gained)}</color>";
                profit.text = $"<color={(net >= 0 ? Good : Bad)}>{Signed(net)}</color>";
                footer.color = net >= 0 ? GoodBg : BadBg;
            }
            catch (Exception e) { MelonLogger.Error("Profit Calculator: " + e); }
        }
    }
}
