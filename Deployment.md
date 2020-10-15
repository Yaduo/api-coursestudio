----------------------- Database Docker 
1.pull sql server image
sudo docker pull microsoft/mssql-server-linux
2.run docker
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=PaSSword12!' -p 1433:1433 -d microsoft/mssql-server-linux
3.connect and test in MacSqlClient

4.save dock container
docker commit 277f7de9ab1c mssqltes

5.stop docker container
docker stop 277f7de9ab1c

----------------------- local DB
server name: localhost
login: sa
Password: PaSSword12!
ConnectionString: Server=localhost; Database=CourseData; User Id=sa; Password=PaSSword12!;

----------------------- Azure DB
server name: yaduodevdb.database.windows.net
login: yaduo
Password: PaSSword12!
ConnectionString: Server=tcp:yaduodevdb.database.windows.net,1433;Initial Catalog=yaduodevdb;Persist Security Info=False;User ID=yaduo;Password=PaSSword12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

----------------------- Api


