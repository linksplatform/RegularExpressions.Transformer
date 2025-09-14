# Chromium Bug Report #2: Incorrect SVG Alignment Inside Image Element

## Summary
When using external SVG files as sources for `<image>` elements within an SVG, the alignment and positioning of the SVG content is incorrect in Chromium browsers, causing visual misalignment compared to other browsers and the SVG specification.

## Browser Information
- Browser: Chromium (all versions tested)
- Affected browsers: Chrome, Microsoft Edge, Opera  
- Non-affected browsers: Firefox, Safari

## Expected Behavior
External SVG files used as `<image>` sources should be positioned and aligned according to the SVG specification, matching the behavior of other browsers and inline SVG content.

## Actual Behavior
External SVG files used as `<image>` sources are misaligned, appearing shifted or positioned incorrectly compared to:
1. The same SVG content when embedded inline
2. How the same code renders in Firefox and Safari
3. Expected positioning according to SVG specifications

## Impact
This forces developers to:
- Avoid external SVG files and inline all SVG content instead
- Create larger HTML files with embedded SVG data
- Lose benefits of SVG file caching and modularity
- Implement workarounds that may affect performance

## Test Case
See attached test files:
- `svg-alignment-test.html` - Demonstrates the alignment issue with external SVG files
- `inline-vs-external-comparison.html` - Shows difference between inline and external SVG positioning

## Reproduction Steps
1. Create an SVG file with specific viewBox and content
2. Reference this SVG as the `href` attribute of an `<image>` element within another SVG
3. Compare the positioning with the same content embedded inline
4. Test in Firefox/Safari to see correct behavior
5. Test in Chromium to observe misalignment

## Additional Notes
This issue was discovered while creating interactive programming language logos for documentation. The current workaround involves embedding all SVG content directly in the HTML file instead of using external SVG files.

Current production example (with workaround): https://linksplatform.github.io/RegularExpressions.Transformer/

## Related Standards
According to the SVG specification, external SVG files referenced via `<image>` elements should be positioned consistently with their inline equivalents.