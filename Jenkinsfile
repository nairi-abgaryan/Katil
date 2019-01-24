pipeline {
  agent any
  
  stages {
    stage('Build Image') {
      steps {
        openshiftBuild(bldCfg: 'case-management', namespace: 'rtb-dms-tools', verbose: 'true')
        openshiftBuild(bldCfg: 'pdf-generator', namespace: 'rtb-dms-tools', verbose: 'true')
        openshiftBuild(bldCfg: 'email-generator', namespace: 'rtb-dms-tools', verbose: 'true')
        openshiftBuild(bldCfg: 'email-notification', namespace: 'rtb-dms-tools', verbose: 'true')
      }
    }

    stage('Deliver to Dev') {
      steps {
        openshiftDeploy(depCfg: 'case-management', namespace: 'rtb-dms-dev', verbose: 'true')
        openshiftDeploy(depCfg: 'pdf-generator', namespace: 'rtb-dms-dev', verbose: 'true')
        openshiftDeploy(depCfg: 'email-generator', namespace: 'rtb-dms-dev', verbose: 'true')
        openshiftDeploy(depCfg: 'email-notification', namespace: 'rtb-dms-dev', verbose: 'true')
      }
    }
  }
  post {
    failure {
        mail (to: 'mikeharlow@hive1-cs.com', subject: "FYI: Job '${env.JOB_NAME}' (${env.BUILD_NUMBER}) failed", body: "Job failed. See ${env.BUILD_URL} for details. ");
        mail (to: 'tigranarakelyan@hive1-cs.com', subject: "FYI: Job '${env.JOB_NAME}' (${env.BUILD_NUMBER}) failed", body: "Job failed. See ${env.BUILD_URL} for details. ");
    }
    success {
        mail (to: 'mikeharlow@hive1-cs.com', subject: "FYI: Job '${env.JOB_NAME}' (${env.BUILD_NUMBER}) deployed to dev", body: "Deploy job succeeded. See ${env.BUILD_URL} for details. ");
    }
  }
}