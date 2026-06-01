# Schedule 1 - Profit Calculator & Analisador Tático

Este repositório documenta a evolução completa de uma ferramenta de inteligência de dados para o jogo de simulação estratégica **Schedule 1**. O projeto nasceu da necessidade de decifrar as mecânicas ocultas de lucratividade do jogo e evoluiu de um script de engenharia reversa para um mod injetável totalmente funcional.

🎮 **[Download do Mod no Nexus Mods](https://www.nexusmods.com/schedule1/mods/2147)**

---

## 🚀 A Trajetória do Projeto

### Fase 1: O Protótipo (Python)
Inicialmente, o projeto era apenas um protótipo baseado em texto localizado na pasta raiz (`schedule.py`). Ele foi construído para decifrar a matemática do jogo e validar a lógica de empilhamento de efeitos.
* **Mapeamento:** Uso de dicionários para emular ingredientes, regras de substituição e preços da Wiki.
* **Validação:** Descoberta da fórmula oficial de precificação: 
  $$Preço = Round(B \times (1 + \sum multiplicadores))$$

### Fase 2: O Mod Oficial (C# / Unity)
Com a lógica validada, o projeto foi totalmente reescrito em **C#** (localizado na pasta `/Source C#`) para se tornar um mod real utilizando o ecossistema **BepInEx** e **Harmony**.
* **Injeção de Código:** Hooks criados com Harmony patches no método `MixingStation` para ler a memória do jogo em tempo real.
* **Interface Gráfica Dinâmica:** Criação de componentes UI nativos do Unity (`TextMeshProUGUI`, `RectTransform`) que se atualizam automaticamente conforme os itens são manipulados nos slots do laboratório.

---

## ✨ Funcionalidades Finais do Mod
* **Cálculo em Tempo Real:** Exibe o custo total, receita bruta e o lucro líquido real antes de iniciar a mistura.
* **Simulação de Multiplicadores:** Calcula o impacto exato de efeitos combinados na precificação final do produto.
* **Estética Integrada:** O painel herda elementos visuais do próprio jogo para manter a imersão.

## 🛠️ Tecnologias Utilizadas
* **Linguagens:** Python (Prototipagem) | C# (Produção)
* **Frameworks & Ferramentas:** Unity Engine, BepInEx 6 (IL2CPP), Harmony Lib, TextMeshPro.

---

## 💡 Disclaimer
Este projeto tem fins estritamente educacionais, servindo como portfólio prático de Engenharia Reversa, estruturas de dados, desenvolvimento de mods e manipulação de interfaces gráficas em tempo real (UI).
