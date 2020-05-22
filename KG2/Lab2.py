from PIL import Image
import glob
from dataclasses import dataclass
import sys

mode_depth = {
    "1": 1,
    "L": 8, 
    "P": 8, 
    "RGB": 24, 
    "YCbCr": 24,
    "HSV": 24, 
    "RGBA": 32, 
    "CMYK": 32, 
    "LAB": 24, 
    "I": 32, 
    "F": 32
    }

for image in map(Image.open, glob.glob(sys.argv[1] + '/*')):
    width, height = image.size
    print("image name: ", image.filename)
    print("image width: ", str(width))
    print("image height: ", str(height))
    print("image dpi: ", image.info.get("dpi"))
    print("image color depth: ", mode_depth[image.mode])
    print("image compression: ", image.info.get('compression'), "\n\n\n")