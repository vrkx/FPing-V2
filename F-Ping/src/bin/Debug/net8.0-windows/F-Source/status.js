  async function getFortniteStatus() {
        const statusList = document.getElementById('status-container');
        const epicStatusApiUrl = 'https://status.epicgames.com/api/v2/summary.json';
        const fortniteServices = ['Fortnite', 'LEGO Fortnite', 'Fortnite Festival', 'UEFN', '', 'Account Services', '', 'Epic Games Store', 'Epic Online Services' , 'Fortnite Crew', '',  ];

  
        try {
            const response = await fetch(epicStatusApiUrl);
            const data = await response.json();

            statusList.innerHTML = ''; // Clear the loading message

            const components = data.components;
            const relevantComponents = components.filter(component => fortniteServices.includes(component.name));

            if (relevantComponents.length > 0) {
                relevantComponents.forEach(component => {
                    const statusItem = document.createElement('div');
                    statusItem.classList.add('server-item');

                    const statusNameSpan = document.createElement('span');
                    statusNameSpan.classList.add('status-name');
                    statusNameSpan.textContent = component.name;

                    const statusIndicator = document.createElement('div');
                    statusIndicator.classList.add('status-indicator', component.status);

                    statusItem.appendChild(statusNameSpan);
                    statusItem.appendChild(statusIndicator);
                    statusList.appendChild(statusItem);
                });
            } else {
                statusList.innerHTML = `<p class="loading-message">No Fortnite-related services found in the API.</p>`;
            }

        } catch (error) {
            statusList.innerHTML = `<p class="loading-message" style="color: #f44336;">Failed to load status. Please try again later.</p>`;
            console.error("Error fetching Epic Games status:", error);
        }
    }

    document.addEventListener("DOMContentLoaded", () => {
       
        getFortniteStatus();
    });



    setInterval(getFortniteStatus, 60000);