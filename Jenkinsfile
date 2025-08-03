pipeline {
    agent any

    stages {
        stage('Build and Test in Docker') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:8.0.8').inside {
                        sh 'dotnet restore'
                        sh 'dotnet build --configuration Release'
                    }
                }
            }
        }
        stage('Run Application in Docker') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:8.0.8').inside {
                        sh 'dotnet run --configuration Release --no-build'
                    }
                }
            }
        }
    }
}
