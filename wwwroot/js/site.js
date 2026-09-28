// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(() => {
    let toastTimer;

    function showToast(message) {
        let toast = document.querySelector(".taply-toast");
        if (!toast) {
            toast = document.createElement("div");
            toast.className = "taply-toast";
            toast.setAttribute("role", "status");
            toast.setAttribute("aria-live", "polite");
            document.body.appendChild(toast);
        }
        toast.textContent = message;
        toast.classList.add("show");
        window.clearTimeout(toastTimer);
        toastTimer = window.setTimeout(() => toast.classList.remove("show"), 1900);
    }

    async function copyText(value) {
        try {
            await navigator.clipboard.writeText(value);
            showToast("Đã sao chép");
        } catch {
            const field = document.createElement("textarea");
            field.value = value;
            field.style.position = "fixed";
            field.style.opacity = "0";
            document.body.appendChild(field);
            field.select();
            const copied = document.execCommand("copy");
            field.remove();
            showToast(copied ? "Đã sao chép" : "Không thể sao chép");
        }
    }

    document.addEventListener("click", event => {
        const copyButton = event.target.closest("[data-copy]");
        if (copyButton) {
            copyText(copyButton.dataset.copy || "");
            return;
        }

        const toggleButton = event.target.closest("[data-toggle-secret]");
        if (!toggleButton) return;
        const code = toggleButton.closest(".info-card")?.querySelector(".door-code");
        if (!code) return;
        const visible = code.textContent === code.dataset.code;
        code.textContent = visible ? "••••••" : code.dataset.code;
        toggleButton.innerHTML = visible
            ? '<i class="bi bi-eye"></i><span>Hiện mã</span>'
            : '<i class="bi bi-eye-slash"></i><span>Ẩn mã</span>';
        toggleButton.setAttribute("aria-label", visible ? "Hiện mã" : "Ẩn mã");
    });
})();
