from itertools import combinations_with_replacement

ingredients = {
    'Cuke': {
        'Preço': 2, 
        'Efeito Base': 'Energizing', 
        'Effect Replacements': {
            'Euphoric': 'Laxative', 
            'Foggy': 'Cyclopean', 
            'Gingeritis': 'Thought-Provoking', 
            'Munchies': 'Athletic', 
            'Slippery': 'Munchies', 
            'Sneaky': 'Paranoia', 
            'Toxic': 'Euphoric'
        }
    }, 
    'Banana': {
        'Preço': 2, 
        'Efeito Base': 'Gingeritis', 
        'Effect Replacements': {
            'Calming': 'Sneaky', 
            'Cyclopean': 'Thought-Provoking', 
            'Disorienting': 'Focused', 
            'Energizing': 'Thought-Provoking', 
            'Focused': 'Seizure-Inducing', 
            'Long Faced': 'Refreshing', 
            'Paranoia': 'Jennerising', 
            'Smelly': 'Anti-Gravity', 
            'Toxic': 'Smelly'
        }
    }, 
    'Paracetamol': {
        'Preço': 3, 
        'Efeito Base': 'Sneaky', 
        'Effect Replacements': {
            'Calming': 'Slippery', 
            'Electrifying': 'Athletic', 
            'Energizing': 'Paranoia', 
            'Focused': 'Gingeritis', 
            'Foggy': 'Calming', 
            'Glowing': 'Toxic', 
            'Munchies': 'Anti-Gravity', 
            'Paranoia': 'Balding', 
            'Spicy': 'Bright-Eyed', 
            'Toxic': 'Tropic Thunder'
        }
    }, 
    'Donut': {
        'Preço': 3, 
        'Efeito Base': 'Calorie-Dense', 
        'Effect Replacements': {
            'Anti-Gravity': 'Slippery', 
            'Balding': 'Sneaky', 
            'Calorie-Dense': 'Explosive', 
            'Focused': 'Euphoric', 
            'Jennerising': 'Gingeritis', 
            'Munchies': 'Calming', 
            'Shrinking': 'Energizing'
        }
    }, 
    'Viagor': {
        'Preço': 4, 
        'Efeito Base': 'Tropic Thunder', 
        'Effect Replacements': {
            'Athletic': 'Sneaky', 
            'Disorienting': 'Toxic', 
            'Euphoric': 'Bright-Eyed', 
            'Laxative': 'Calming', 
            'Shrinking': 'Gingeritis'
        }
    }, 
    'Mouth Wash': {
        'Preço': 4, 
        'Efeito Base': 'Balding', 
        'Effect Replacements': {
            'Calming': 'Anti-Gravity', 
            'Calorie-Dense': 'Sneaky', 
            'Explosive': 'Sedating', 
            'Focused': 'Jennerising'
        }
    }, 
    'Flu Medicine': {
        'Preço': 5, 
        'Efeito Base': 'Sedating', 
        'Effect Replacements': {
            'Athletic': 'Munchies', 
            'Calming': 'Bright-Eyed', 
            'Cyclopean': 'Foggy', 
            'Electrifying': 'Refreshing', 
            'Euphoric': 'Toxic', 
            'Focused': 'Calming', 
            'Laxative': 'Euphoric', 
            'Munchies': 'Slippery', 
            'Shrinking': 'Paranoia', 
            'Thought-Provoking': 'Gingeritis'
        }
    }, 
    'Gasoline': {
        'Preço': 5, 
        'Efeito Base': 'Toxic', 
        'Effect Replacements': {
            'Disorienting': 'Glowing', 
            'Electrifying': 'Disorienting', 
            'Energizing': 'Euphoric', 
            'Euphoric': 'Spicy', 
            'Gingeritis': 'Smelly', 
            'Jennerising': 'Sneaky', 
            'Laxative': 'Foggy', 
            'Munchies': 'Sedating', 
            'Paranoia': 'Calming', 
            'Shrinking': 'Focused', 
            'Sneaky': 'Tropic Thunder'
        }
    }, 
    'Energy Drink': {
        'Preço': 6, 
        'Efeito Base': 'Athletic', 
        'Effect Replacements': {
            'Disorienting': 'Electrifying', 
            'Euphoric': 'Energizing', 
            'Focused': 'Shrinking', 
            'Foggy': 'Laxative', 
            'Glowing': 'Disorienting', 
            'Schizophrenic': 'Balding', 
            'Sedating': 'Munchies', 
            'Spicy': 'Euphoric', 
            'Tropic Thunder': 'Sneaky'
        }
    }, 
    'Motor Oil': {
        'Preço': 6, 
        'Efeito Base': 'Slippery', 
        'Effect Replacements': {
            'Energizing': 'Munchies', 
            'Euphoric': 'Sedating', 
            'Foggy': 'Toxic', 
            'Munchies': 'Schizophrenic', 
            'Paranoia': 'Anti-Gravity'
        }
    }, 
    'Mega Bean': {
        'Preço': 7, 
        'Efeito Base': 'Foggy', 
        'Effect Replacements': {
            'Athletic': 'Laxative', 
            'Calming': 'Glowing', 
            'Energizing': 'Cyclopean', 
            'Focused': 'Disorienting', 
            'Jennerising': 'Paranoia', 
            'Seizure-Inducing': 'Focused', 
            'Shrinking': 'Electrifying', 
            'Slippery': 'Toxic', 
            'Sneaky': 'Calming', 
            'Thought-Provoking': 'Energizing'
        }
    }, 
    'Chili': {
        'Preço': 7, 
        'Efeito Base': 'Spicy', 
        'Effect Replacements': {
            'Anti-Gravity': 'Tropic Thunder', 
            'Athletic': 'Euphoric', 
            'Laxative': 'Long Faced', 
            'Munchies': 'Toxic', 
            'Shrinking': 'Refreshing', 
            'Sneaky': 'Bright-Eyed'
        }
    }, 
    'Battery': {
        'Preço': 8, 
        'Efeito Base': 'Bright-Eyed', 
        'Effect Replacements': {
            'Cyclopean': 'Glowing', 
            'Electrifying': 'Euphoric', 
            'Euphoric': 'Zombifying', 
            'Laxative': 'Calorie-Dense', 
            'Munchies': 'Tropic Thunder', 
            'Shrinking': 'Munchies'
        }
    }, 
    'Iodine': {
        'Preço': 8, 
        'Efeito Base': 'Jennerising', 
        'Effect Replacements': {
            'Calming': 'Balding', 
            'Calorie-Dense': 'Gingeritis', 
            'Euphoric': 'Seizure-Inducing', 
            'Foggy': 'Paranoia', 
            'Refreshing': 'Thought-Provoking', 
            'Toxic': 'Sneaky'
        }
    }, 
    'Addy': {
        'Preço': 9, 
        'Efeito Base': 'Thought-Provoking', 
        'Effect Replacements': {
            'Explosive': 'Euphoric', 
            'Foggy': 'Energizing', 
            'Glowing': 'Refreshing', 
            'Long Faced': 'Electrifying', 
            'Sedating': 'Gingeritis'
        }
    }, 
    'Horse Semen': {
        'Preço': 9, 
        'Efeito Base': 'Long Faced', 
        'Effect Replacements': {
            'Anti-Gravity': 'Calming', 
            'Gingeritis': 'Refreshing', 
            'Thought-Provoking': 'Electrifying', 
            'Seizure-Inducing': 'Energizing'
        }
    }
}

multiplicadores = {
    'Shrinking': 1.60,
    'Zombifying': 1.58,
    'Cyclopean': 1.56,
    'Anti-Gravity': 1.54,
    'Long Faced': 1.52,
    'Electrifying': 1.50,
    'Glowing': 1.48,
    'Tropic Thunder': 1.46,
    'Thought-Provoking': 1.44,
    'Jennerising': 1.42,
    'Bright-Eyed': 1.40,
    'Spicy': 1.38,
    'Foggy': 1.36,
    'Slippery': 1.34,
    'Athletic': 1.32,
    'Balding': 1.30,
    'Calorie-Dense': 1.28,
    'Sedating': 1.26,
    'Sneaky': 1.24,
    'Energizing': 1.22,
    'Gingeritis': 1.20,
    'Euphoric': 1.18,
    'Focused': 1.16,
    'Refreshing': 1.14,
    'Munchies': 1.12,
    'Calming': 1.10,
    'Disorienting': 1.00,
    'Explosive': 1.00,
    'Laxative': 1.00,
    'Lethal': 1.00,
    'Paranoia': 1.00,
    'Schizophrenic': 1.00,
    'Seizure-Inducing': 1.00,
    'Smelly': 1.00,
    'Toxic': 1.00
}

marijuana = {
    'OG Kush': {
        'Preço': 38,
        'Efeito Base': 'Calming'},
    'Sour Diesel': {
        'Preço': 40,
        'Efeito Base': 'Refreshing'},
    'Green Crack': {
        'Preço': 43,
        'Efeito Base': 'Energizing'},
    'Granddaddy Purple': {
        'Preço': 44,
        'Efeito Base': 'Sedating'}}

sinteticos = {
    'Meta': {
        'Preço': 70},
    'Cogumelo': {
        'Preço': 100},
    'Coca': {
        'Preço': 150}}

soma = 0
efeitos_atuais = []

droga_base = input('Qual é a droga base? (OG Kush, Sour Diesel, Green Crack, Granddaddy Purple, Meta, Cogumelo ou Coca) ')

if droga_base in marijuana:
    soma += marijuana[droga_base]['Preço']
    efeitos_atuais.append(marijuana[droga_base]['Efeito Base'])
elif droga_base in sinteticos:
    soma += sinteticos[droga_base]['Preço']   
else:
    print('Droga Invalida!')
    exit()

etapa_ingrediente = 1
while True:
    ing = input(f'Qual e o {etapa_ingrediente}º ingrediente: ')

    if ing in ingredients:
        soma += ingredients[ing]['Preço']
        substituicoes_do_ingrediente = ingredients[ing]['Effect Replacements']

        for i in range(len(efeitos_atuais)):
            efeito_sendo_analisado = efeitos_atuais[i]
            if efeito_sendo_analisado in substituicoes_do_ingrediente:
                efeitos_atuais[i] = substituicoes_do_ingrediente[efeito_sendo_analisado]
            
        efeitos_atuais.append(ingredients[ing]['Efeito Base'])

        etapa_ingrediente += 1
    else:
        break


efeitos_atuais.sort()
print(f'\n✨Efeitos na Droga: {efeitos_atuais}')

valores_multiplicadores = []
for efeito in efeitos_atuais:
    valores_multiplicadores.append(multiplicadores[efeito])

print(f'✖ Multiplicadores encontrados: {valores_multiplicadores}')
print(f'💲Custo Total de Produção: ${soma}')

if droga_base in marijuana:
    b_equacao = 35
else:
    b_equacao = sinteticos[droga_base]['Preço']

soma_multiplicadores = 0
for valor in valores_multiplicadores:
    soma_multiplicadores += round(valor - 1.0, 2)

preco_venda = round(b_equacao * (1 + soma_multiplicadores))

print(f'\n💰 PREÇO DE VENDA SUGERIDO: ${preco_venda}')
print(f'💰 LUCRO LÍQUIDO: ${preco_venda - soma}')

def encontrar_melhor_mistura(base_nome, qtd_ings):
    melhor_lucro_local = -999
    melhor_combo_local = None
    melhor_preco_local = 0
    melhor_custo_local = 0

    lista_nomes_ings = list(ingredients.keys())
    combinacoes = combinations_with_replacement(lista_nomes_ings, qtd_ings)

    for combo in combinacoes:

        soma_teste = 0
        efeitos_teste = []
        
        if base_nome in marijuana:
            soma_teste += marijuana[base_nome]['Preço']
            efeitos_teste.append(marijuana[base_nome]['Efeito Base'])
            b_teste = 35
        else:
            soma_teste += sinteticos[base_nome]['Preço']
            b_teste = sinteticos[base_nome]['Preço']

        for item in combo:
            soma_teste += ingredients[item]['Preço']
            regras = ingredients[item]['Effect Replacements']
            for i in range(len(efeitos_teste)):
                if efeitos_teste[i] in regras:
                    efeitos_teste[i] = regras[efeitos_teste[i]]
            efeitos_teste.append(ingredients[item]['Efeito Base'])
        if len(efeitos_teste) != len(set(efeitos_teste)):
           continue

        soma_multi_teste = sum(round(multiplicadores[e] - 1.0, 2) for e in efeitos_teste)
        preco_teste = round(b_teste * (1 + soma_multi_teste))
        lucro_teste = preco_teste - soma_teste

        if lucro_teste > melhor_lucro_local:
            melhor_lucro_local = lucro_teste
            melhor_combo_local = combo
            melhor_preco_local = preco_teste
            melhor_custo_local = soma_teste

    return melhor_combo_local, melhor_lucro_local, melhor_preco_local, melhor_custo_local


print("\n" + "-"*30)
pergunta_otimizar = input("Deseja saber qual a melhor mistura para esta droga base? (s/n): ")
if pergunta_otimizar.lower() == 's':
    qtd = int(input("Quantos ingredientes serao utilizados na busca? "))
    combo_vencedor, lucro_vencedor, preco_vencedor, custo_vencedor = encontrar_melhor_mistura(droga_base, qtd)
    print(f"\n🏆 A melhor mistura encontrada para {droga_base} com {qtd} ingredientes é:")
    print(f"👉 {' + '.join(combo_vencedor)}")
    print(f"💲Custo Total de Produção: ${custo_vencedor}")

    print(f"\n💰 PREÇO DE VENDA SUGERIDO: ${preco_vencedor}")
    print(f"💰 LUCRO LÍQUIDO: ${lucro_vencedor}")








  