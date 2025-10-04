// Initialize highlight.js
document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelectorAll('pre code').forEach((block) => {
        hljs.highlightElement(block);

        // Add language label
        const language = block.className.match(/language-(\w+)/);
        if (language && language[1]) {
            const langLabel = document.createElement('span');
            langLabel.className = 'code-language-label';
            langLabel.textContent = language[1].toUpperCase();
            block.parentNode.insertBefore(langLabel, block);
        }

        // Add copy button
        const copyButton = document.createElement('button');
        copyButton.className = 'copy-code-button';
        copyButton.textContent = 'Copy';
        block.parentNode.insertBefore(copyButton, block);

        copyButton.addEventListener('click', () => {
            const codeText = block.textContent;
            navigator.clipboard.writeText(codeText).then(() => {
                copyButton.textContent = 'Copied!';
                setTimeout(() => {
                    copyButton.textContent = 'Copy';
                }, 2000);
            }).catch(err => {
                console.error('Failed to copy code: ', err);
            });
        });
    });
});

// Toggle password visibility
function setupPasswordToggle(inputId, toggleButtonId) {
    const passwordInput = document.getElementById(inputId);
    const toggleButton = document.getElementById(toggleButtonId);

    if (passwordInput && toggleButton) {
        toggleButton.addEventListener('click', () => {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            // Toggle the eye icon (optional, requires changing the SVG path)
            // For now, we'll just toggle the type
        });
    }
}

setupPasswordToggle('passwordInput', 'togglePasswordVisibility'); // Login
setupPasswordToggle('passwordInputRegister', 'togglePasswordVisibilityRegister'); // Register Password
setupPasswordToggle('confirmPasswordInputRegister', 'toggleConfirmPasswordVisibilityRegister'); // Register Confirm Password
setupPasswordToggle('passwordInputAdmin', 'togglePasswordVisibilityAdmin'); // Admin Login