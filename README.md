# Profit Calculator (mod para Schedule I)

Mostra na Mixing Station quanto uma mistura vai render antes de você apertar BEGIN.

[Download no Nexus Mods](https://www.nexusmods.com/schedule1/mods/2147)

## No jogo

Ao colocar um produto e um mixer na estação, aparece um painel ao lado com:

- o produto que sai da mistura e os efeitos dele, nas cores do jogo;
- o mixer e a quantidade que vai ser misturada;
- o preço por unidade depois da mistura, com o preço atual entre parênteses;
- quanto custam os mixers;
- quanto a mistura acrescenta ao valor do lote;
- o lucro líquido: o valor acrescentado menos o custo dos mixers. Fica vermelho quando a mistura não compensa.

Os preços saem das mesmas funções que o jogo usa (`ProductManager.CalculateProductValue` e `EffectMixCalculator.MixProperties`). Quando a combinação é uma receita conhecida, o painel usa o produto e o preço base dessa receita.

## Requisitos

- Schedule I 0.4.7f12 na versão Mono, a branch `alternate` da Steam. A versão IL2CPP padrão não funciona.
- MelonLoader 0.7.

## Compilar e instalar

```
dotnet build -c Release
```

Copie `bin\Release\net472\ProfitCalculator.dll` para a pasta `Mods` do jogo, com o jogo fechado. O caminho do jogo está em `GameDir`, no `ProfitCalculator.csproj`.

## Histórico

O projeto começou como um script em Python (`schedule.py`) para entender como o jogo calcula o preço dos produtos. Inspecionei o código do jogo com o dnSpy e montei dicionários com ingredientes, regras de troca de efeitos e preços da wiki para testar a lógica de efeitos combinados.

A versão 1 foi um plugin para BepInEx 6 na versão IL2CPP do jogo (0.4.5). Ela usava Harmony para ler os slots da Mixing Station e montava o painel com TextMeshPro.

A versão 2 foi reescrita para MelonLoader na versão Mono do jogo (branch `alternate`). Além da troca de loader, ela:

- acompanha a interface nova da estação (`MixingStationInterface`), que substituiu a antiga na 0.4.7;
- calcula o preço com a fórmula atual do jogo, que também considera o `ValueChange` e o `ValueMultiplier` de cada efeito;
- compara com o preço do produto sem mistura, em vez de contar a receita inteira como lucro;
- tem um painel novo, que cresce conforme o número de efeitos.

O código da versão 1 continua no histórico do repositório.

## Tecnologias

C# (.NET Framework 4.7.2), MelonLoader, Harmony, Unity UI e TextMeshPro. O protótipo é em Python.
