Follow the steps to setup the local environment pointing to the Gold database running on local machine.

1- Open Command Prompt in Administrative mode.

2- Change directory path to the Scenario Testing folder inside Services folder in Aumentum TFS  path.

3- Run 'get.aumentum.from.master.bat'. Run this only once and only when you need to pull the whole aumentum site from dev 2 to local site. This may take around 1-2 minutes, depending on the network speed. Unzip this file to c:\local_aa\sites\aumentum\ folder.

4- Run 'get.bvs.from.master.bat' only when new BVS UI related changes are needed to be pulled from Dev 2 site. This is not required if setting the local for the first time as this is already done in Step 3.

5- Run 'get.all.micro.services.from.master.bat'. This will take some time as it pulls down the all the service code from Dev 1 server. This may take around 5 minutes, depending on the network speed.

6- 