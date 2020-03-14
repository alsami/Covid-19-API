#!/usr/bin/env bash
function create_image() {
    if [[ -z ${TRAVIS_TAG} ]]; then
        docker build -t covid19api.latest -t ${REGISTRY}/covid19api:latest -f ./src/covid19api/Dockerfile .
    else 
        docker build -t covid19api.latest -t ${REGISTRY}/covid19api:latest -t ${REGISTRY}/covid19api:${TRAVIS_TAG} -f ./src/covid19api/Dockerfile .
    fi
}

function publish_image() {
    if [[ -n ${TRAVIS_TAG} ]]; then
        docker push ${REGISTRY}/covid19api:${TRAVIS_TAG}
    fi
    
    docker push ${REGISTRY}/covid19api:latest
}

create_image
publish_image