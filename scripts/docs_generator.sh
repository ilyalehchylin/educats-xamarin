# Install DocFX
choco install docfx --version=2.51

# Remove previous version of documentation
rm -r docs/

# Copy all files & directories inside pages to the root
cp -r pages/* ./

# Clone material template
git clone https://github.com/ovasquez/docfx-material.git material

# Set environment variables
DOCFX_SOURCE_BRANCH_NAME=master

# Generate docs metadata
docfx metadata docfx.json

# Build docs
docfx build docfx.json -o docs

# Change theme colors
sh scripts/docs_theme_edit.sh

# Remove redundant directories
rm -rf api/ material/ obj/ articles/ index.md toc.yml favicon.ico logo.svg docs/styles/*.backup
