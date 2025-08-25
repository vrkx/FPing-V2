
import refresh from './core.js';


document.addEventListener('DOMContentLoaded', () => {
    
    try {
    
            const clientId = '1409303719610286193'; // your client id
        
            const redirectUri = 'https://fpingbackend.vercel.app/api/callback'; // your discird dashbiard redirect uri ( has to be exact)
            const setupscreenmodal = document.getElementById('setup-screen-modal');
            const mainAppScreen = document.getElementById('main-app-screen');
            const loginButton = document.getElementById('discord-login-btn');
            const loginSection = document.getElementById('login-section');
            const profileSection = document.getElementById('profile-section');
            const userPfp = document.getElementById('userpfp');
            const usernameDisplay = document.getElementById('UserSName');

    
            const discordAuthUrl = `https://discord.com/api/oauth2/authorize?client_id=${clientId}&response_type=code&redirect_uri=${encodeURIComponent(redirectUri)}&scope=identify guilds.join`;
    
            loginButton.href = discordAuthUrl;

   
            const savedUsername = localStorage.getItem('discordUsername');
            const savedPfp = localStorage.getItem('discordPfp');

            if (savedUsername && savedPfp) {
         
                    mainAppScreen.style.display = 'flex';
                    mainAppScreen.style.visibility = 'visible';
                    mainAppScreen.style.visibility = 'visible';
                    setupscreenmodal.style.display = 'none';
                     refresh();
                    setupscreenmodal.style.visibility = 'hidden';
                usernameDisplay.textContent = savedUsername;
                userPfp.src = savedPfp;
            } else {
           
                const urlParams = new URLSearchParams(window.location.search);
                const username = urlParams.get('username');
                const pfp = urlParams.get('pfp');

                if (username && pfp) {
               
                    localStorage.setItem('discordUsername', username);
                    localStorage.setItem('discordPfp', pfp);
                    


                    mainAppScreen.style.display = 'none';
                    mainAppScreen.style.visibility = 'hidden';
                    setupscreenmodal.style.display = 'flex';
                    setupscreenmodal.style.visibility = 'visible';
                    refresh();

                    
                    usernameDisplay.textContent = username;
                    userPfp.src = pfp;

                
                    window.history.replaceState({}, document.title, window.location.pathname);
                } else {
              
                    setupscreenmodal.style.display = 'flex';
                    setupscreenmodal.style.visibility = 'visible';
                    mainAppScreen.style.display = 'none';
                    mainAppScreen.style.visibility = 'hidden';
                }
            }
        }
    
    catch (error) {
        console.error('Error during Discord login process:', error);
    }
}
);