#!/usr/bin/env bash
function create_image() {
    if [[ -z ${TRAVIS_TAG} ]]; then
        docker build -t covid-19-api.latest -t ${REGISTRY}/covid-19-api:latest -f ./src/Covid19Api/Dockerfile .
    else 
        docker build -t covid-19-api.latest -t ${REGISTRY}/covid-19-api:latest -t ${REGISTRY}/covid-19-api:${TRAVIS_TAG} -f ./src/Covid19Api/Dockerfile .
    fi
}

function publish_image() {
    if [[ -n ${TRAVIS_TAG} ]]; then
        docker push ${REGISTRY}/covid-19-api:${TRAVIS_TAG}
    fi
    
    docker push ${REGISTRY}/covid-19-api:latest
}

create_image
publish_image