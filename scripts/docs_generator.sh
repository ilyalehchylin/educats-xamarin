# Copy all files & directories inside pages to the root
cp -r pages/* ./

# Clone material template
git clone https://github.com/ovasquez/docfx-material.git material

# Generate docs metadata
docfx metadata docfx.json

# Build docs
docfx build docfx.json -o docs

# Change theme colors
sed -i '.backup' -e 's/#5e92f3/#27AEE1/g' docs/styles/main.css
sed -i '.backup' -e 's/#0d47a1/#0D6D92/g' docs/styles/main.css
sed -i '.backup' -e 's/#003c8f/#0D6D92/g' docs/styles/main.css
sed -i '.backup' -e 's/#fffaef/#F0F8FF/g' docs/styles/main.css
sed -i '.backup' -e 's/#795da3/#0D6D92/g' docs/styles/docfx.vendor.css
sed -i '.backup' -e 's/#a71d5d/#27AEE1/g' docs/styles/docfx.vendor.css

# Remove redundant directories
rm -rf api/ material/ obj/ articles/ index.md toc.yml favicon.ico logo.svg docs/styles/*.backup
