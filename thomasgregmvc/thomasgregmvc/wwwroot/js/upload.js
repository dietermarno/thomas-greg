const input = document.getElementById("fileSelector");
const avatar = document.getElementById("preview");
const textArea = document.getElementById("base64");

const convertBase64 = (file) => {
    return new Promise((resolve, reject) => {
        const fileReader = new FileReader();
        fileReader.readAsDataURL(file);

        fileReader.onload = () => {
            resolve(fileReader.result);
        };

        fileReader.onerror = (error) => {
            reject(error);
        };
    });
};

const uploadImage = async (event) => {
    const file = event.target.files[0];
    const base64 = await convertBase64(file);
    avatar.src = base64;
    textArea.innerText = base64;
};

input.addEventListener("change", (e) => {
    uploadImage(e);
});