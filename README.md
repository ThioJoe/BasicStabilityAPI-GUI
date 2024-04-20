# Basic Stable Diffusion API GUI

This repository contains an unofficial set of Python scripts to interact with the Stability AI API for generating and upscaling images using their SD3 and SD3-Turbo models.

## Screenshots
<img width="550" alt="Window Screenshot" src="https://github.com/ThioJoe/BasicStabilityAPI-GUI/assets/12518330/23190559-b8b2-4add-a73c-002d414fc498">

## Features

**Generate images using the Stability AI API with customizable settings:**
- Specify text prompts and negative prompts
 - Control the number of images to generate
 - Choose between SD3 and SD3-Turbo models
 - Set the aspect ratio of generated images
 - Manually specify seed or use 0 for random
 - Select the output format (PNG, JPEG, WEBP)
 - Preview generated images in a dedicated window

**Upscale existing images using the Stability AI API:**
- Initiate upscaling requests with customizable parameters
- Automatically retrieve and download upscaled images
- Uses log file which details and tracks downloaded/undownloaded images

**Other Notable Features**:
- Release executables signed with [EV code signing certificate](https://en.wikipedia.org/wiki/Extended_Validation_Certificate) (No pop up from Windows about untrusted software)
- Automatic loading of API key from text file after you enter it the first time


## Requirement - API Key:
- A stability AI API key is required. You can read some instructions here: https://platform.stability.ai/docs/getting-started
- If you already have an account you can find your API key in your account settings here: https://platform.stability.ai/account/keys

## Image Preview Window Screenshot

<img width="550" alt="Preview Window" src="https://github.com/ThioJoe/BasicStabilityAPI-GUI/assets/12518330/48be20a2-81ac-4781-a9d7-5a52e559669a">
