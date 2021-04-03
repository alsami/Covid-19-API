#!/usr/bin/env bash
function authenticate() {
  az login --service-principal -u ${SERVICE_PRINCIPAL_USER} -p ${SERVICE_PRINCIPAL_PASSWORD} --tenant ${SERVICE_PRINCIPAL_TENANT}  
}

function invoke() {
  az iot hub invoke-device-method -n ${IOT_HUB_NAME} -d ${IOT_DEVICE_NAME} --mn ${IOT_DEVICE_METHOD_NAME} --mp '{ "tag": "'${TRAVIS_TAG}'" }' --to 60
}

authenticate
invoke