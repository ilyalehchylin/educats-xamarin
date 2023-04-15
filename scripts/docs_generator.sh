#!/bin/bash

set -e

# Install DocFX
# brew install docfx
curl -OL https://github.com/dotnet/docfx/releases/download/v2.65.3/docfx-osx-x64-v2.65.3.zip
unzip docfx*.zip -d docfx_tool

# Remove previous version of documentation
rm -r docs/

# Copy all files & directories inside pages to the root
cp -r pages/* ./

# Clone material template
git clone https://github.com/ovasquez/docfx-material.git material

# Set environment variables
DOCFX_SOURCE_BRANCH_NAME=master

# Generate docs metadata
./docfx_tool/docfx metadata docfx.json

# Build docs
./docfx_tool/docfx build docfx.json -o docs

# Change theme colors
sh scripts/docs_theme_edit.sh

# Remove redundant directories
rm -rf api/ material/ obj/ articles/ index.md toc.yml favicon.ico logo.svg docs/styles/*.backup
