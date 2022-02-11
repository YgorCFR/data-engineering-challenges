import pandas as pd
from connect_db import cursor, engine


cursor.execute("SELECT * FROM Sales.Customer;")

SalesCustomerDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

cursor.execute('SELECT * FROM Person.Person;')

PersonPersonDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

cursor.execute('SELECT * FROM Person.EmailAddress;')

PersonEmailAddressDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

cursor.execute('''SELECT pp.*, ppt.Name FROM Person.PersonPhone pp 
      INNER JOIN Person.PhoneNumberType ppt ON pp.PhoneNumberTypeID = ppt.PhoneNumberTypeID;''')

PersonPhoneDf = pd.DataFrame((tuple(t) for t in cursor), columns=[column[0] for column in cursor.description])

print("==== SalesCustomerDf ====")
print(SalesCustomerDf.shape)
print(f"Columns of SalesCustomerDf {SalesCustomerDf.columns}")
print("==== PersonPersonDf ====")
print(PersonPersonDf.shape)
print(f"Columns of PersonPersonDf {PersonPersonDf.columns}")
print("==== PersonEmailAddressDf ====")
print(PersonEmailAddressDf.shape)
print(f"Columns of PersonEmailAddressDf {PersonEmailAddressDf.columns}")
print("==== PersonPersonDf ====")
print(PersonPhoneDf.shape)
print(f"Columns of PersonPhoneDf {PersonPhoneDf.columns}")

cols_to_use = PersonPhoneDf.columns.difference(PersonEmailAddressDf.columns).tolist().append('BusinessEntityID')
JoinContactsDf = pd.merge(PersonEmailAddressDf, PersonPhoneDf, how='inner', on=['BusinessEntityID'])

cols_to_use = JoinContactsDf.columns.difference(PersonPersonDf.columns).tolist().append('BusinessEntityID')
JoinAllPersonExtractedTablesDf = pd.merge(PersonPersonDf, JoinContactsDf, how='inner', on=['BusinessEntityID'])

cols_to_use = SalesCustomerDf.columns.difference(JoinAllPersonExtractedTablesDf.columns)
DimCustomerDf = pd.merge(JoinAllPersonExtractedTablesDf, SalesCustomerDf[cols_to_use.tolist()], how='left', left_on='BusinessEntityID', right_on='PersonID')
# SAVING NEW Customer Dimension
DimCustomerDf.to_sql("Customer", con=engine, schema="Dim", if_exists='replace')
