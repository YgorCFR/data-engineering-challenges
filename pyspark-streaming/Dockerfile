# builder step used to download and configure spark environment
FROM openjdk:11.0.11-jre-slim-buster as builder

# Add Dependencies for PySpark
RUN apt-get update && apt-get install -y curl vim wget software-properties-common ssh net-tools ca-certificates python3 python3-pip python3-numpy python3-matplotlib python3-scipy python3-pandas python3-simpy

RUN python3 -m pip install kafka-python pyspark



RUN update-alternatives --install "/usr/bin/python" "python" "$(which python3)" 1

# Fix the value of PYTHONHASHSEED
# Note: this is needed when you use Python 3.3 or greater
ENV SPARK_VERSION=3.0.2 \
HADOOP_VERSION=3.2 \
SPARK_HOME=/opt/spark \
PYTHONHASHSEED=1

# Download and uncompress spark from the apache archive
RUN wget --no-verbose -O apache-spark.tgz "https://archive.apache.org/dist/spark/spark-${SPARK_VERSION}/spark-${SPARK_VERSION}-bin-hadoop${HADOOP_VERSION}.tgz" \
&& mkdir -p /opt/spark \
&& tar -xf apache-spark.tgz -C /opt/spark --strip-components=1 \
&& rm apache-spark.tgz

# Apache spark environment
FROM builder as apache-spark

WORKDIR /opt/spark

ENV SPARK_MASTER_PORT=7077 \
SPARK_MASTER_WEBUI_PORT=8080 \
SPARK_LOG_DIR=/opt/spark/logs \
SPARK_MASTER_LOG=/opt/spark/logs/spark-master.out \
SPARK_MASTER="spark://spark-master:7077" \
SPARK_WORKLOAD="master"

EXPOSE 8080 7077 6066

RUN mkdir -p $SPARK_LOG_DIR && \
touch $SPARK_MASTER_LOG && \
ln -sf /dev/stdout $SPARK_MASTER_LOG 


COPY start-spark.sh /
COPY opt/spark/conf/log4j.properties /opt/spark/conf/

RUN wget https://repo1.maven.org/maven2/org/apache/spark/spark-sql-kafka-0-10_2.12/3.0.2/spark-sql-kafka-0-10_2.12-3.0.2.jar && \
    mv spark-sql-kafka-0-10_2.12-3.0.2.jar /opt/spark/jars

RUN wget https://repo1.maven.org/maven2/org/apache/kafka/kafka-clients/3.0.2/kafka-clients-3.0.2.jar && \
    mv kafka-clients-3.0.2.jar /opt/spark/jars

RUN wget https://repo1.maven.org/maven2/org/apache/commons/commons-pool2/2.11.1/commons-pool2-2.11.1.jar && \
    mv commons-pool2-2.11.1.jar /opt/spark/jars

RUN wget https://repo1.maven.org/maven2/org/apache/spark/spark-token-provider-kafka-0-10_2.12/3.0.2/spark-token-provider-kafka-0-10_2.12-3.0.2.jar && \
    mv spark-token-provider-kafka-0-10_2.12-3.0.2.jar /opt/spark/jars

CMD ["/bin/bash", "/start-spark.sh"]
