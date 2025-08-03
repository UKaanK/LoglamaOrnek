pipeline {
    agent any

    stages {
        stage('Build and Test in Docker') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:7.0').inside {
                        sh 'dotnet restore'
                        sh 'dotnet build --configuration Release'
                        sh 'dotnet test'
                    }
                }
            }
        }
        stage('Run Application in Docker') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:7.0').inside {
                        sh 'dotnet run --configuration Release --no-build'
                    }
                }
            }
        }
    }
}
