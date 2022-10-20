from pyspark.sql import SparkSession
from pyspark.sql.types import StructType, StructField, StringType, FloatType
from pyspark.sql.functions import from_json,col


soft_drink_schema = StructType([
    StructField("brand", StringType(), False),
    StructField("price", FloatType(), False)
])

spark = SparkSession.\
        builder\
        .appName('AppExample')\
        .getOrCreate()

df = spark.readStream\
    .format("kafka")\
    .option("kafka.bootstrap.servers", "kafka:9093")\
    .option("subscribe", "topic-test")\
    .option("startingOffsets", "earliest")\
    .load()
    
     
df1 = df.select(col("value").cast("string").alias("value"))\
        .select(from_json(col("value"), soft_drink_schema).alias("data"))\
        .select("data.*")

df1.printSchema()

# df1.writeStream\
#   .format("console")\
#   .outputMode("append")\
#   .start()\
#   .awaitTermination()


destination_workload = df1.writeStream\
   .format("parquet")\
   .outputMode("append")\
   .option("path", "results/content")\
   .partitionBy("brand")\
   .trigger(processingTime='30 seconds')\
   .option("checkpointLocation", "results/checkpoint")\
   .start()

destination_workload.awaitTermination()