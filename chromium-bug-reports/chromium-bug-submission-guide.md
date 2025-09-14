# Chromium Bug Submission Guide

## Overview
This document provides instructions for submitting bug reports to the Chromium project for the two rendering issues discovered in the RegularExpressions.Transformer project.

## Bug Reports to Submit

### Bug #1: Small SVG Image Elements Don't Render
- **File**: `bug-report-1-small-images.md`
- **Test Case**: `examples/small-image-test.html`
- **Priority**: Medium
- **Component**: Blink>SVG

### Bug #2: Incorrect SVG Alignment in Image Elements  
- **File**: `bug-report-2-svg-alignment.md`
- **Test Cases**: `examples/svg-alignment-test.html`, `examples/test-logo.svg`
- **Priority**: Medium
- **Component**: Blink>SVG

## Submission Steps

### Prerequisites
1. Create a Google Account if you don't have one
2. Verify the issues in the latest Chrome Canary build
3. Test the issues in other browsers (Firefox, Safari) to confirm they're Chromium-specific

### Step-by-Step Submission

1. **Go to the Chromium Issue Tracker**
   - Visit: https://issues.chromium.org/issues
   - Or use the shortcut: https://crbug.com/new

2. **Sign In**
   - Click "Sign In" and use your Google Account

3. **Create New Issue**
   - Click "New Issue" or "Create Issue"

4. **Fill Out the Form**

   **For Bug #1 (Small Images):**
   - **Summary**: "SVG image elements with dimensions < 0.5px don't render"
   - **Component**: Blink>SVG
   - **Priority**: P3 (Medium)
   - **Type**: Bug
   - **Description**: Copy content from `bug-report-1-small-images.md`
   - **Attachments**: Upload `examples/small-image-test.html`

   **For Bug #2 (SVG Alignment):**
   - **Summary**: "Incorrect positioning of external SVG files in image elements"
   - **Component**: Blink>SVG
   - **Priority**: P3 (Medium)
   - **Type**: Bug
   - **Description**: Copy content from `bug-report-2-svg-alignment.md`
   - **Attachments**: Upload `examples/svg-alignment-test.html` and `examples/test-logo.svg`

5. **Additional Information to Include**
   - **OS**: Your operating system
   - **Browser Version**: Latest Chrome stable and Canary versions tested
   - **Reproducible**: Yes
   - **Frequency**: Always

### Template Information for Each Bug

#### Bug #1 Template:
```
Component: Blink>SVG
Summary: SVG image elements with dimensions < 0.5px don't render
Description: [Content from bug-report-1-small-images.md]
Steps to reproduce:
1. Create SVG with image element having width/height < 0.5px
2. Open in Chrome
3. Compare with Firefox/Safari
Expected result: Image should render if visible after scaling
Actual result: Image doesn't render at all
```

#### Bug #2 Template:
```
Component: Blink>SVG  
Summary: Incorrect positioning of external SVG files in image elements
Description: [Content from bug-report-2-svg-alignment.md]
Steps to reproduce:
1. Create external SVG file
2. Reference in image element with specific x,y,width,height
3. Compare positioning with inline SVG
4. Test in Chrome vs Firefox/Safari
Expected result: External SVG positioned same as inline
Actual result: External SVG appears misaligned
```

## Post-Submission

1. **Note the Issue Numbers**
   - Save the bug report URLs for reference
   - Update this repository with the issue numbers

2. **Monitor Progress**
   - Check for developer responses
   - Provide additional information if requested
   - Test proposed fixes when available

3. **Update Documentation**
   - Document the bug report numbers in the repository
   - Update the pull request description with links to the Chromium issues

## Important Notes

- Be respectful and professional in all communications
- Provide detailed reproduction steps and test cases
- Be responsive if developers ask for clarification
- Don't duplicate existing reports - search first
- Keep descriptions focused and technical

## Expected Timeline

- **Initial Response**: 1-7 days
- **Triage**: 1-4 weeks  
- **Investigation**: Varies widely
- **Fix**: Depends on complexity and priority

## References

- Chromium Bug Reporting Guidelines: https://www.chromium.org/for-testers/bug-reporting-guidelines/
- Chromium Issue Tracker: https://issues.chromium.org/issues
- SVG Specification: https://www.w3.org/TR/SVG2/