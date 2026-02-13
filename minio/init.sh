#!/bin/sh
set -e

mc alias set local http://minio:9000 minioadmin minioadmin
mc mb -p local/images
mc anonymous set download local/images
