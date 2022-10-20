# Demo pyspark streaming com docker compose e kafka.
Consiste em uma demo para simular uma aplicação de pyspark streaming com o uso do docker compose e kafka.

## Requisitos:
- Docker desktop.

## Procedimento:
- Estando na raíz do projeto.
- Execute:
```bash
docker build -t cluster-apache-spark:3.0.2 .
```
- Em seguida, execute o docker compose:
```bash
docker-compose up
```
- Com todos os conteineres online, acesse o conteiner do pyspark, dirigindo-se ao caminho /opt/spark-apps/
- Execute o producer.py
```bash
python producer.py
```
- Abra outro terminal e se dirija ao mesmo caminho /opt/spark-apps/
```bash
spark-submit example.py \ 
  --packages org.apache.spark:spark-sql-kafka-0-10_2.12:3.0.2 \
  --jars /opt/spark/jars/kafka-clients-3.0.2.jar,/opt/spark/jars/commons-pool2-2.11.1.jar,/opt/spark/jars/spark-token-provider-kafka-0-10_2.12-3.0.2.jar
```
