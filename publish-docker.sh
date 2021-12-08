#!/usr/bin/env bash
function create_image() {
    if [[ -z ${TAG} ]]; then
        docker build -t covid-19-api:latest -t ${REGISTRY}/covid-19-api:latest -f ./src/Covid19Api/Dockerfile .
    else 
        docker build -t ${REGISTRY}/covid-19-api:${TAG} -f ./src/Covid19Api/Dockerfile .
    fi
}

function publish_image() {
    if [[ -n ${TAG} ]]; then
        docker push ${REGISTRY}/covid-19-api:${TAG}
    fi
    
    #docker push ${REGISTRY}/covid-19-api:latest
}

create_image
publish_image