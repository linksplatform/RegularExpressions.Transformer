# Chromium Bug Report #1: SVG Image Elements with Small Dimensions Don't Render

## Summary
SVG `<image>` elements with height or width less than 0.5 pixels do not render in Chromium-based browsers, even when the parent SVG is scaled up through CSS or viewport transformations.

## Browser Information
- Browser: Chromium (all versions tested)
- Affected browsers: Chrome, Microsoft Edge, Opera
- Non-affected browsers: Firefox, Safari

## Expected Behavior
SVG `<image>` elements should render regardless of their specified dimensions, as long as they are visible in the final rendered output after transformations and scaling.

## Actual Behavior
SVG `<image>` elements with dimensions smaller than 0.5 pixels are not rendered at all, even when the parent SVG container is scaled to make them visually larger than 0.5 pixels.

## Impact
This forces developers to use workarounds like scaling up all dimensions by a factor of 10 or more, which:
- Makes code less intuitive and harder to maintain
- Requires manual coordinate system conversion
- May cause precision issues with very small or very large numbers
- Breaks the natural 1:1 relationship between design specifications and code

## Test Case
See attached test files:
- `small-image-test.html` - Demonstrates the issue
- `comparison-browsers.html` - Shows how other browsers handle this correctly

## Reproduction Steps
1. Create an SVG with an `<image>` element having width/height < 0.5
2. Place this SVG in a scaled container or viewport
3. Observe that the image doesn't render in Chromium browsers
4. Test the same code in Firefox/Safari to see it works correctly

## Additional Notes
This issue was discovered while developing interactive SVG diagrams for documentation. The workaround (10x scaling) is currently implemented in production at: https://linksplatform.github.io/RegularExpressions.Transformer/

## Related Standards
According to SVG specifications, there should be no minimum size limit for rendered elements, only for what's visually perceptible.