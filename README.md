# DivvyExercise

Installation Instructions:
1. Download and install Terraform (instructions can be found online, outside the scope of this project https://www.terraform.io/downloads.html)
2. Open a Terminal and move current directory to ~/DOWNLOAD_LOCATION/DivvyExercise/infrastructure
3. Run the command "terraform init" in the terminal to initialize the terraform (download's AWS Provider)
4. Run the command "terraform plan" in the terminal to validate all 19 objects to be created
5. Run the command "terraform apply" in the terminal to create all infrastructure.
6. Log into S3 AWS Console and upload the file ~/DOWNLOAD_LOCATION/DivvyExercise/sample_data_file.txt
7. Go to DynamoDb AWS Console and validate items are in table.
