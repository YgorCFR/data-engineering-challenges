## To execute these scripts
1. Check the SQL Server connection
2. Create a python virtual environment with the following command:
```python
py -m venv sandbox
```
3. Enter in the virtual environment:
on Windows: 
```bash
cd sandbox/
cd Scripts/
activate
```
on Linux:
```bash
source sandbox/Scripts/activate
```

4. Install the python dependencies contained in the requirements.txt file:
```bash
pip install -r requirements.txt
```

5. Open the SQL Server Management studio and create the Schemas, (Fact and Dim) with the following script:
```sql
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Fact')
BEGIN
EXEC('CREATE SCHEMA Fact')
END

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Dim')
BEGIN
EXEC('CREATE SCHEMA Dim')
END
```

6. To feed the Fact.Sales:
```
py fact_sales_feed.py
```

7. To feed the Dim.Customer:
```
py dim_customer_feed.py
```
