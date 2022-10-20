1. To run this program, make sure you have SQL Server LocalDB installed.
2. Open the cmd and connect to the LocalDB with the following command:
	```bash
	sqlcmd -S (localdb)\MSSQLLocalDB
	```
3. Then run the following query in order to create the exercise's database:
   ```sql
	IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Q2Db')
      BEGIN
        CREATE DATABASE [Q2Db]
      END
   ```
   ```sql
   GO
   ```
5. Open the visual studio 2019 through the Q2App.sln.
6. Press the light green "Play" button of the IDE to run the Program.