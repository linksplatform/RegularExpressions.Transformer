# Chromium Bug Reports for RegularExpressions.Transformer

This directory contains documentation and test cases for submitting bug reports to the Chromium project regarding SVG rendering issues discovered in the RegularExpressions.Transformer project.

## Background

The RegularExpressions.Transformer project's GitHub Pages site (https://linksplatform.github.io/RegularExpressions.Transformer/) contains interactive SVG diagrams that required workarounds for Chromium browser rendering issues. These workarounds were implemented in commits:

- [89c4f13](https://github.com/linksplatform/RegularExpressions.Transformer/commit/89c4f13652d18cfda7179c381a0bbabf6931bdfa) - Stroke width scaled 10x larger
- [7b29f0e](https://github.com/linksplatform/RegularExpressions.Transformer/commit/7b29f0ecd34dcab9ec2ce403c267ac8648b05653) - All sizes scaled up 10x  
- [d7659aa](https://github.com/linksplatform/RegularExpressions.Transformer/commit/d7659aa139d828f5ba06bacc2c892b766799519c) - SVG images embedded directly instead of external files

## Issues Identified

### 1. Small SVG Elements Don't Render
**Problem**: SVG elements with dimensions smaller than 0.5 pixels don't render in Chromium browsers, even when scaled up via CSS transforms.

**Impact**: Forces developers to artificially scale up coordinates by 10x or more.

**Files**: 
- `bug-report-1-small-images.md` - Detailed bug report
- `examples/small-image-test.html` - Test case demonstrating the issue

### 2. SVG Alignment Issues with External Files  
**Problem**: External SVG files referenced via `<image>` elements are incorrectly positioned compared to inline SVG content.

**Impact**: Forces developers to embed all SVG content inline instead of using modular external files.

**Files**:
- `bug-report-2-svg-alignment.md` - Detailed bug report  
- `examples/svg-alignment-test.html` - Test case showing misalignment
- `examples/test-logo.svg` - Sample external SVG file for testing

## Submission Instructions

Follow the guide in `chromium-bug-submission-guide.md` to submit these bug reports to the Chromium project.

**Quick Links**:
- Chromium Issue Tracker: https://issues.chromium.org/issues
- New Issue: https://crbug.com/new

## Testing the Issues

1. Open the test HTML files in different browsers:
   - **Chrome/Edge**: Will show the bugs
   - **Firefox/Safari**: Will show correct rendering

2. Compare the differences to understand the issues

3. Use these test cases when submitting bug reports

## File Structure

```
chromium-bug-reports/
├── README.md                           # This file
├── chromium-bug-submission-guide.md   # Step-by-step submission guide
├── bug-report-1-small-images.md       # Bug report for small elements issue
├── bug-report-2-svg-alignment.md      # Bug report for alignment issue
└── examples/
    ├── small-image-test.html          # Test case for small elements
    ├── svg-alignment-test.html        # Test case for alignment
    └── test-logo.svg                  # Sample external SVG
```

## Next Steps

1. Test the examples in multiple browsers to verify the issues
2. Submit bug reports using the provided templates and guides
3. Update this README with the Chromium issue numbers once submitted
4. Monitor the bug reports for developer responses

## References

- Original issue: https://github.com/linksplatform/RegularExpressions.Transformer/issues/17
- Project GitHub Pages: https://linksplatform.github.io/RegularExpressions.Transformer/
- Chromium Bug Guidelines: https://www.chromium.org/for-testers/bug-reporting-guidelines/