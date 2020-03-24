echo "Starting changing theme process..."

# Change navigation bar background color
sed -i '.backup' -e 's/#0d47a1/#333333/g' docs/styles/main.css

# Change links colors
sed -i '.backup' -e 's/#003c8f/#27AEE1/g' docs/styles/main.css

# Change links hover colors
sed -i '.backup' -e 's/#5e92f3/#0D6D92/g' docs/styles/main.css

# Change syntax block background color
sed -i '.backup' -e 's/#fffaef/#F0F8FF/g' docs/styles/main.css

# Change syntax block title color
sed -i '.backup' -e 's/#795da3/#0D6D92/g' docs/styles/docfx.vendor.css

# Change syntax block keywords color
sed -i '.backup' -e 's/#a71d5d/#27AEE1/g' docs/styles/docfx.vendor.css

# Change code block background color
sed -i '.backup' -e 's/#f9f2f4/#F0F8FF/g' docs/styles/docfx.vendor.css

# Change code block text color
sed -i '.backup' -e 's/#c7254e/#27AEE1/g' docs/styles/docfx.vendor.css

echo "Changing theme process completed."
