# JurassicJigsaw
Advent of Code - Day 20 puzzle

## Summary

The code in this repository was developed to solve the challenge for the Advent of Code 2020, Day 20 challenge.

The problem statement this code addresses loosely involves image reconstruction from broken "image" data.
The image reconstruction problem posed in the challenge is fairly close to a normal table top jigsaw puzzle, 
with a single image, split apart into many smaller pieces that then get reassembled with their shapes and image content
show that they should go next to each other. 

The actual challenge here in Advent Day 20 is slightly more complicated, with additional issues around: 
1) tile flipping and rotation, and
2) a second part, where, once the main image is reconstructed, you do some **object detection** within the final image.

Images here are just from text files, and we do not use more image-specific approaches, no RGB, no CYMK color spaces, no specific image libraries needed.

The advent site has a fuller description here [https://adventofcode.com/2020/day/20]

Why did we pick this Advent problem?

I picked it because it's a simplified problem. So should not need specific industry or firm knowledge to understand the problem or the code. 
Other folks chose and vetted the problem. 
Because it's simplified, there's no infrastructure or data dependencies that would have to get solved for in order for code to be shared, run.
Also, we picked it because it was there. I wanted a different way to engage with and be a part of the broader developer community.

These Advent challenges are also fun because (as I am discovering), they way they split between Part I and Part II, often resembles how requirements either shift or get extended as project development rolls along. So, it's interseting to see how stable your code is as the problem shifts under you. 

Aspects of code, software dev exemplified here:
- 2d-matrix types of operations
- a simplified approach to a sliding window object recognition algorithm.

## Code organization
Overall, the code here is broken up into different areas: **Models** for the data objects and **Services** for the main problem-solving logic. An **IO** section handles basic file-level operations.

The main method in Program is basically a script that walks thru the problem solving steps, and outputs some information in the middle. The actual answer to the challenge is just one number. And while direct, it's not very informative for any humans that may want to read along. 

### Models
Main models broken down into a set of three related classes: a Tile, a TileFrame, and a TileFrameSet. 
Their heirarchy is as follows: 
- TileFrameSet
    - TileFrame
        - Tile
        
The Tile basically holds the image content for a single piece. The TileFrame and the TileFrameSet help in organizing individual Tiles so that they can form a larger, single, coherent image. 

Code-wise, the Tile is also the level where we handle image rotation, and image flipping. 

## Tests
Within the code solution in the repo, UnitTests and IntegrationTests are organized into separate c# level project structures. Test suites implemented with Xunit.

### Unit tests
Unit tests here primarily covering either individual Tile transformation type methods (rotations), or detection type methods.

### Integration tests (and some end-to-end)
 - checks actual FileIO + data interpretation issues
 - some end-to-end solving for actual test case

### Final tests
Final testing was done manually with the challenge samples and web UI at Advent of Code. 

### More input data
In the repo, in the InputData folder, there are two samples input files: 
1. sampleinput.txt - a 9 tile (3x3) example where expected answers were provided in the challenge
2. input.txt - the "test set" input, where the correct answer was not provided.

## Acknowledgements
Special thanks to Eric Wastl and the team over at Advent of Code for putting together the event, coming up with interesting puzzles, and trying to promote a healthy dev community. 
