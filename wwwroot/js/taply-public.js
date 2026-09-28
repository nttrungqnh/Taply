(() => {
    const toast = document.getElementById("taplyToast");
    let toastTimeout;

    function showToast(message) {
        if (!toast) return;
        toast.textContent = "✓ " + message;
        toast.classList.add("is-visible");
        window.clearTimeout(toastTimeout);
        toastTimeout = window.setTimeout(() => toast.classList.remove("is-visible"), 2000);
    }

    async function copyTaplyValue(value, message) {
        try {
            if (navigator.clipboard && window.isSecureContext) {
                await navigator.clipboard.writeText(value);
            } else {
                const input = document.createElement("textarea");
                input.value = value;
                input.setAttribute("readonly", "");
                input.style.position = "fixed";
                input.style.opacity = "0";
                document.body.appendChild(input);
                input.select();
                const copied = document.execCommand("copy");
                input.remove();
                if (!copied) throw new Error("Copy failed");
            }
            showToast(message);
        } catch {
            showToast("Không thể sao chép");
        }
    }

    function toggleSensitiveValue(button) {
        const value = button.closest(".taply-card")?.querySelector("[data-sensitive='true']");
        if (!value) return;
        const isHidden = value.textContent !== value.dataset.value;
        value.textContent = isHidden ? value.dataset.value : "•••••";
        const label = button.querySelector("span");
        if (label) label.textContent = isHidden ? "Ẩn mã" : "Hiện mã";
        button.setAttribute("aria-label", isHidden ? "Ẩn mã" : "Hiện mã");
        const icon = button.querySelector("i");
        if (icon) icon.className = isHidden ? "bi bi-eye-slash" : "bi bi-eye";
    }

    window.copyTaplyValue = copyTaplyValue;
    window.toggleSensitiveValue = toggleSensitiveValue;

    document.addEventListener("click", event => {
        const copyButton = event.target.closest("[data-copy]");
        if (copyButton) {
            copyTaplyValue(copyButton.dataset.copy || "", copyButton.dataset.copyMessage || "Đã sao chép");
            return;
        }
        const toggleButton = event.target.closest("[data-toggle-sensitive]");
        if (toggleButton) toggleSensitiveValue(toggleButton);
    });
})();
