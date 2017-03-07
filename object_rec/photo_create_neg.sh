counter=1;
text="gen.jpg";
echo "Moving into directory $1";
originalDir=$(pwd);
cd $1

echo "Resizing images to 25%";
for i in $( ls ); do
    convert $i -resize 25% "$i";
    let counter+=1;
done;


echo "Converting to grayscale";
for i in $( ls ); do
    convert $i -colorspace Gray "$i";
    let counter+=1;
done;

echo "Creating flipped images"
mkdir ../generated_images
for i in $( ls ); do
    convert $i -rotate 180 "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Creating blurred images"
for i in $( ls ); do
    convert $i -blur 1x2 "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Creating brighter images"
for i in $( ls ); do
    convert $i -brightness-contrast 0.5 "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Creating darker images"
for i in $( ls ); do
    convert $i -brightness-contrast -0.5 "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Creating higher contrast images images"
for i in $( ls ); do
    convert $i -contrast "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Creating lower contrast images images"
for i in $( ls ); do
    convert $i +contrast "../generated_images/$counter$text";
    let counter+=1;
done;

echo "Cleaning up generated_images folder";
mv ../generated_images/* .;
rm -r ../generated_images;

echo "Generating negative.txt file";
cd ..;
ls "negatives/*.jpg" > negatives.txt;
