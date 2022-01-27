import pandas as pd
from connect_db import cursor, engine





cursor.execute('SELECT * FROM Sales.SalesOrderHeader;')

SalesOrderHeaderDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

cursor.execute('SELECT * FROM Sales.SalesOrderDetail')

SalesOrderDetailDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

print("==== SalesOrderDetailDf ====")
print(SalesOrderDetailDf.shape)
print(f"Columns of SalesOrderDetailDf {SalesOrderDetailDf.columns}")
print("==== SalesOrderHeaderDf ====")
print(SalesOrderHeaderDf.shape)
print(f"Columns of SalesOrderHeaderDf {SalesOrderHeaderDf.columns}")

cols_to_use = SalesOrderHeaderDf.columns.difference(SalesOrderDetailDf.columns).tolist().append('SalesOrderID')
FactSalesDf = pd.merge(SalesOrderDetailDf, SalesOrderHeaderDf, how="left", on=['SalesOrderID'])
# SAVING new Fact Sales
FactSalesDf.to_sql("Sales", con=engine, schema="Fact", if_exists='replace')


