# Exemplos de JSON para testar os endpoints

## Fabricante - POST /api/fabricantes

```json
{
  "nome": "Honda",
  "paisOrigem": "Japão"
}
```

## Categoria - POST /api/categorias

```json
{
  "nome": "Pickup",
  "descricao": "Veículos para transporte de cargas e uso misto"
}
```

## Cliente - POST /api/clientes

```json
{
  "nome": "Carlos Souza",
  "cpf": "11122233344",
  "email": "carlos@email.com",
  "telefone": "31988887777"
}
```

## Veículo - POST /api/veiculos

```json
{
  "placa": "JKL1M23",
  "modelo": "Civic",
  "anoFabricacao": 2024,
  "quilometragem": 12000,
  "valorDiaria": 190,
  "disponivel": true,
  "fabricanteId": 1,
  "categoriaId": 3
}
```

## Aluguel - POST /api/alugueis

```json
{
  "clienteId": 1,
  "veiculoId": 1,
  "dataInicio": "2026-10-01T08:00:00",
  "dataFim": "2026-10-05T18:00:00"
}
```

## Devolução - POST /api/alugueis/1/devolucao

```json
{
  "dataDevolucao": "2026-10-05T17:30:00",
  "quilometragemFinal": 35600
}
```

## Pagamento - POST /api/pagamentos

```json
{
  "aluguelId": 1,
  "dataPagamento": "2026-10-01T08:10:00",
  "valor": 720,
  "formaPagamento": "PIX",
  "status": "Pago"
}
```
