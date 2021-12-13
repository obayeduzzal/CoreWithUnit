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
            echo 'Starting Job'
            emailext(
                     to: 'pbayed.khandaker@brainstation-23.com',
                     subject: "Started Job '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
                     body: "Started Job '${env}'"
                )
        }
        stage('Restore'){
            steps{
                options{
                    timeout(time: 1, unit: 'HOURS')
                }
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
        stage('Test'){
            steps{
                echo 'Running Unit Test'
                bat "dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='c:/JenkinsData/CoverageReport/'"
            }
        }
        stage('UAT Publish'){
            when{
                brnach 'master'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish -c release'
            }
        }
        stage('Prod Publish'){
            when{
                branch 'Production'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish -c release'
            }
        }
        stage('Delete Previous'){
            steps{
                bat 'rmdir /s /q "C:\\CICD\\Deployment\\Pipeline"'
            }
        }
        stage('copy'){
            steps{
                bat'robocopy C:\\Windows\\System32\\config\\systemprofile\\AppData\\Local\\Jenkins\\.jenkins\\workspace\\CoreJenkins\\CoreJenkins\\CoreJenkins\\bin\\Release\\net5.0\\publish C:\\CICD\\Deployment\\Pipeline /e & EXIT /B 0'
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