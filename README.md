# README #

This README would normally document whatever steps are necessary to get your application up and running.

### What is this repository for? ###
Course Studio API
Course Studio Domain
Course Studio Data
Course Studio Lib

### How do I get set up? ###
1. open solution by Visual Studio
2. build solution
3. if localDB not exisist, run "update-database" in Package Manager Concole 
4. set CourseStudio.API as start up project

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

### Who do I talk to? ###

* Repo owner or admin
* Other community or team contact



领域服务与应用服务的区别
http://www.cnblogs.com/sheng-jie/p/7097129.html

https://www.cnblogs.com/landeanfen/p/4844344.html



run local, not access the local database
1. 
docker inspect bridge 找出database的ip address
修改webconfig
docker build -f ./Presentation/CourseStudio.Api/Dockerfile -t coursestudio-api .
docker run -d -p 51176:80 --name coursestudio-api  coursestudio-api 

2. 
docker build -f ./Presentation/CourseStudioManager.Api/Dockerfile -t coursestudiomanager-api .
docker run --rm -d -p 8080:80 --name coursestudiomanager-api  coursestudiomanager-api 