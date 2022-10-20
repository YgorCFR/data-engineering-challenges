import random 
import decimal
from time import sleep
from json import dumps
from kafka import KafkaProducer 

# Defining brands
brands = ["CocaColaZero", "Johnny Walker", "Guinnes", "7up"]

producer = KafkaProducer(
    bootstrap_servers=['kafka:9093'],
    value_serializer=lambda x: dumps(x).encode('utf-8')
)

for i in range(10000):
    price = float(decimal.Decimal(random.randrange(4, 9)))
    data = {
        "brand": random.choice(brands),
        "price": price
    }
    producer.send('topic-test', value=data)
    sleep(0.5)