#!/usr/bin/env bash

function invoke() {
  az iot hub invoke-device-method -n ${IOT_HUB_NAME} -d ${IOT_DEVICE_NAME} --mn ${IOT_DEVICE_METHOD_NAME} --mp '{ "tag": "'${TAG}'" }' --to 60
}

invoke