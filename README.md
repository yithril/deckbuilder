# Template for netcore 3.1 lambda without all the extra junk

## Requirements
- AWS Account
- Auth0 Account
- AWS CLI installed on your machine
- This Repo
- NodeJS installed on your machine
- DotNetCore 3.1 installed on your machine
- A code editor
    - recommended VSCode for the pieces of this not directly related to C#
    - recommended visual studio 2019 (2017 or newer is fine)

## How To:
    - Create an AWS account. [Amazon Site click here](https://aws.amazon.com)
        - If you need a credit card to provide you can create a safe and secure card from [Privacy Secure Credit Card Site](https://www.privacy.com)
        - Create an AWS account
            - NOTE: After logging into aws in the upper right of the screen is a region. Always be looking here as sometimes it will change randomly. you want us-east-2 or US ohio as this is the primary region used for this guide.
            1. Follow whatever steps AWS asks you to follow to create an account. You will need a credit card but will not get charged until you use a service that costs money
                - Be very careful when working with things. you can pretty much try out anything but you should delete it right after your doing using it.
                - Using a language like terraform or serverless framework or cloud formation to spin up all your resources will help you quickly tear down when your done
            2. Once your are signed into your AWS account and at your AWS console windows i suggest clicking services at the top and navigating to IAM
            3. Once there, there is a security status. It has 4-5 things that you should do. Do them all. You can skips the groups for now if you don't have groups or users you want to create other then yourself
            4. click services at the top and navigate to route53
            5. click registered domains on the left and purchase or transfer in your first domain. You do not have to have a purchased domain here but I believe its cheaper to use a amazon purchased domain then it is to use an external domain
            6. If you purchased a domain it will spin up a hosted zone for you. this costs $0.50 cents a month to exist. It is the place that holds your DNS records.
            7. If you are transfering in a domain, you will need to manually have to create your hosted zone. Either way the zone costs 50 cents a month but, i believe, there are extra charges associated do domains not owned by you at amazon.
            8. navigate back to IAM and create a new IAM user with programatic access. It should have guided you thought this in the beginning. To keep things simple you can give it admin access but you should restrict it to exactly what your going to be using it for. Up to you at this point. Get your access and secret key and note them down.
            9. now we need to create SSL certificates for the domain you purchased which should be ready by now.
            10. from the aws console click services and search for acm. FYI these are free SSL certs....thats right, HTTPS for free. 
            11. From provision certificates click get started and request a public certificate.
            12. Put in your domain name with a * for a wildcard. You should be a lot more specific with this but this will get you started. For me it is going to be *.cavanaughexamples.com and click next. 
            13. Select the option for DNS Validation and click next
            14. Add any tags if you want. These are key value pairs then click review
            15. validate everything looks right and click confirm and request
            16. A validation screen will pop up and show your domain is pending validation. DO NOT LEAVE THIS SCREEN.
            17. Click the arrow next to your domain to drop it down.
            18. Click create record in route 53 (the record is free) to add the validation record to your DNS which is required for your cert to be valid. A screen will pop up then click create. Now you can click continue and leave the screen i told you not to leave.
            19. it may take up to an hour for your cert to validate but most likely no more then 15 minutes.

    - Create an Auth0 Account to secure your endpoint. You can build custom login or security but this is safer and helps avoid charges
        - Can be used for service to service auth or shared account auth for services for no cost
        - https://www.auth0.com
        - Follow the steps below to setup an app in auth0
            1. Create an Auth0 Account
            2. Create a tenant url (it may prompt you for this)
            3. on the left hand sidebar click API's
            4. click create API button
            5. provide a name for your API. (environment specific) Example: Test-API or Prod-API
            6. Provide your applications URL (you should have a domain by now. If you don't this might not work very well)
            7. Leave/set your Signing Algorithm at/to RS256
            8. You should be re-directed to your API. 
            9. Click the 'test' tab on the API screen and note down your client_id, client_secret, audience, and grant_type in the curl example
                - See postman collection in this repo for an example query on getting a request token to use against your API
    - Setting up your PC:
        - Navigate to https://www.nodejs.org and install the latest LTS version of node (currently 12.18.3). This enabled the serverless framework IAC and deployment tools you will be using
        - Navigate to https://dotnet.microsoft.com/download/dotnet-core/3.1 and install the latest version of netcore 3.1. This enables you to build and work with dotnet framework API. You might already have this if you have visual studio installed and up to day.
        - Pull this repo down to your machine, navigate into the NetCoreTemplateAPI folder and from your command prompt do a npm install to update all npm packages. Again this is required for deployment
        - Open the projects solution file and build your netcore code and download all your nuget packages.
        - navigate to https://aws.amazon.com/cli/ and download from the links on the right side the most up to date version of the aws CLI for whatever OS you have.
        - After aws CLI is installed open a command windows and run AWS configure --profile {{WHATEVERYOUWANTTOCALLTHISPROFILETOIDENFIYTHEAWSACCOUNTYOUJUSTCREATED}} and hit enter
            - it will ask you for your clientid, client secret default region (us-east-2 or whatever you want) and response type or whatever the last question is which is json
    - Setting up your deployment environment
        - Navigate to and open the serverless.yml file in this repo.
            - Decide what environments you are going to have. Currently setup for test and prod. You will see many "maps" in the file where you can create an environment for each one you will have or only one. Put your default deployment environment in the quotes on line 11 for stage and the profile name you created a few steps ago in the quotes for line 12. You can skip this if you want because during the deploy we can inject the profile name but if it is always going to stay the same then putting it here is easier.
            - put in your domain you want to use in the domain map starting on line 39. In the example case i purchased cavanaughexamples.com do the domain i'm going to use for my test domain map is dotnetapi-test.cavanaughexamples.com and prod would be dotnetapi.cavanaughexamples.com. you can make whatever you want for this.
            - Put in your cert you created in the cert map. remember above we create *.cavanaughexamples.com. Add this for both test and prod assuming it is the cert you created.
            - in the Auth0DomainMap add in the auth0 domain you had created earlier. Mine was https://mcavanaugh-example.us.auth0.com and it will be this for both test and prod in this case. yours might be different if you created for them one tenant
    - Finishing touches:
        - navigate back to the aws console, go to services and search for ssm. go to systems manager
        - on the left hand side panel click Parameter store as we are going to store our auth0 audience here.
        - click create parameter
        - for your name enter /dotnetapi/test/auth0Audience   << you can adjust this however you want but it must start with a / and not end with a /. If you change the name adjust the audience string in the serverless.yml file. If you have a different audience for prod create a different secret in the param store to match it only checking the environment. the environment here has to match the environment you have setup in serverless.yml file.
        - after naming your secret you can add a description if you want. Under Type Select SecureString. This will encrypt the variable you are going to enter after saving. In value paste your audience then hit the create parameter button.

## Deploying your service
    - Notice there is a build.cmd script in this repo. These command will help you build your service.
    - Before running this, for the first deploy in each environment on each domain name your deploying from your command console run serverless create_domain -s {{ENVIRONMENT HERE}}


    