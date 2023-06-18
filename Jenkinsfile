pipeline {
  agent any
  stages {
    stage('Checkout code') {
      steps {
        git(url: 'https://github.com/Tola200010/product_api', branch: 'main')
      }
    }

    stage('Test Unit Test') {
      steps {
        sh '''
cd Product.Api/ProductApi.UnitTests/ 

#!/bin/bash
dotnet build
dotnet test
'''
      }
    }

    stage('Build') {
      steps {
        sh '''#!/bin/bash
docker build .
'''
      }
    }

  }
}