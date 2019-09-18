# Ramer-Douglas-Peucker algorithm
## Description
An algorithm that decimates a curve composed of line segments to a similar curve with fewer points.

The purpose of the algorithm is, given a curve composed of line segments (which is also called a Polyline in some contexts), to find a similar curve with fewer points. The algorithm defines 'dissimilar' based on the maximum distance between the original curve and the simplified curve (i.e., the Hausdorff distance between the curves). The simplified curve consists of a subset of the points that defined the original curve.

[`source wiki`](https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm)

## Implementation
The [pseudo-code](https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm#Pseudocode) is available on the wikipedia page. In this implementation, we use the [__perpendicular distance__](https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line).

In order to make the algorithm faster, we consider the squared distance (and epsilon) so that we avoid using the _absolute value_ and _square root_ functions in the distance computation. We also split the computation of the distance so that we put in the 'for loop' only what is needed (the denominator is only computed once).

![perpendicular distance from wiki](https://wikimedia.org/api/rest_v1/media/math/render/svg/be2ab4a9d9d77f1623a2723891f652028a7a328d)

## Results

