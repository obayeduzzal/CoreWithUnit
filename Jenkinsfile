pipeline{
    agent {
        label {
            label ""
            customWorkspace "C:/JenkinsData/${JOB_NAME}"
        }
    }
    options {
        timeout(time: 30, unit: 'MINUTES')
    }
    stages{
        stage('Job Start Notification'){
			steps{
				echo 'Starting Job'
				emailext(
						to: 'obayed.khandaker@brainstation-23.com',
						subject: "Job Started '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
						body: "Job Started '${env}'"
					)
			}          
        }
        stage('Restore'){
            steps{
                //options{
                    //timeout(time: 1, unit: 'HOURS')
                //}
                echo 'Restoring Missing Packages'
                bat 'dotnet restore'
            }
        }
        stage('Clean'){
            steps{
                echo 'Cleaning Solution'
                bat 'dotnet clean'
            }
        }
        stage('Build'){
            steps{
                echo 'Building Solution'
                bat 'dotnet build'
            }
        }
        stage('Unit Test UAT'){
			When{
				branch 'master'
			}
            steps{
                echo 'Running Unit Test At Master'
                bat "dotnet test \\CoreWithTest.Test\\CoreWithTest.Test.csproj --collect:"XPlat Code Coverage" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='C:\\JenkinsData\\MultiBrnachTest\\master\\CoreWithTest.Test\\CoverageReport'"
            }
        }
        stage('UAT Publish'){
            when{
                branch 'master'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish \\CoreWithUnit.Api\\CoreWithUnit.Api.csproj -c release'
            }
        }
        stage('Prod Publish'){
            when{
                branch 'Production'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish \\CoreWithUnit.Api\\CoreWithUnit.Api.csproj -c release'
            }
        }
        stage('Delete Previous'){
            steps{
                bat 'rmdir /s /q "C:\\CICD\\Deployment\\MultiBranch"'
            }
        }
        stage('copy'){
            steps{
                bat'robocopy C:\\JenkinsData\\MultiBrnachTest\\master\\CoreWithUnit.Api\\bin\\Release\\net5.0\\publish C:\\CICD\\Deployment\\MultiBranch /e & EXIT /B 0'
            }
        }
    }
    post{
        success {
            echo 'Job Succeded'
            emailext(
                    to: 'obayed.khandaker@brainstation-23.com',
                    subject: "Job Finished '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
                    body: "Job Finished '${env}'"
                )
        }

        failure {
            echo 'Job Failed'
            emailext(
                    to: 'obayed.khandaker@brainstation-23.com',
                    subject: "Job Failed '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
                    body: "Job Failed '${BUILD_URL}''",
                    attachLog: true
                )
        }

    }
}