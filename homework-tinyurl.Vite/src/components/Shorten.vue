<template>
  <div>
    <input type="text" v-model="url" placeholder="Enter URL" />
    <button @click="createUrl">Create URL</button>
    <div v-if="response">
      <p>Response: {{ response }}</p>
    </div>
    <div v-if="error">
      <p>Error: {{ error }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const url = ref(''); // Initialize url as an empty string
const response = ref(null);
const error = ref(null);

const createUrl = async () => {
  response.value = null; // Clear previous response
  error.value = null; // Clear previous error

  try {
    const fetchResponse = await fetch('/api/create', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        url: url.value,
        hash: null,
      }),
    });

    if (!fetchResponse.ok) {
      const errorMessage = await fetchResponse.text(); // Get error text from response
      throw new Error(`HTTP error! status: ${fetchResponse.status}, message: ${errorMessage}`);
    }

    const data = await fetchResponse.json();
    response.value = data;
  } catch (err) {
    error.value = err.message || 'An unexpected error occurred.';
    console.error('Fetch error:', err);
  }
};
</script>