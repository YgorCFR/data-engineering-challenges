import pyodbc
import pandas as pd
from sqlalchemy.engine import URL, create_engine


connection_string = "DRIVER={SQL Server};SERVER=localhost\SQLEXPRESS;DATABASE=AdventureWorks2019;Trusted_Connection=True"
connection_url = URL.create("mssql+pyodbc", query={"odbc_connect": connection_string})

engine = create_engine(connection_url)

conn = pyodbc.connect('Driver={SQL Server};'
'Server=localhost\SQLEXPRESS;'
'Database=AdventureWorks2019;'
'Trusted_Connection=yes;')

cursor = conn.cursor()