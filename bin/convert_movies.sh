sourcedir=../Source/Video/
targetdir=../Assets/Fylgja/Video/
for filename in ${sourcedir}*.mp4; do
    output=${targetdir}$(basename "$filename" .mp4).webm
    echo "from ${filename} to ${output}"
    # Envode to VP8 and Vorbis. Unity has no support for vp9 (`libvpx-vp9`)
    ffmpeg -y -i ${filename} -vcodec libvpx -acodec libvorbis -crf 30 -b:v 1M ${output}
done
