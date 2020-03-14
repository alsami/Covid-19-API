#!/usr/bin/env bash
# https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest
# https://docs.microsoft.com/en-us/cli/azure/webapp/config/container?view=azure-cli-latest#az-webapp-config-container-set
# https://docs.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest#az-webapp-restart
function authenticate() {
    echo authenticating with service-principal
    az login --service-principal -u ${SERVICE_PRINCIPAL_USER} -p ${SERVICE_PRINCIPAL_PASSWORD} --tenant ${SERVICE_PRINCIPAL_TENANT}
}


function set_container() {
    echo updating container to version ${TRAVIS_TAG}
    az webapp config container set -c "${REGISTRY}/covid19api:${TRAVIS_TAG}" -r https://${REGISTRY} -u ${REGISTRY_USER} -p "${REGISTRY_PASSWORD}" -n ${COVID19API_SERVICE_NAME} -g ${COVID19API_RESOURCE_NAME}
}

function restart_app() {
    echo restarting application
    az webapp restart --name ${COVID19API_SERVICE_NAME} --resource-group ${COVID19API_RESOURCE_NAME}
}

authenticate
set_container
restart_app
exit ${?}