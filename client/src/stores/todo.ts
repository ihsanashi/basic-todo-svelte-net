import { writable } from 'svelte/store';
import { Timer } from 'easytimer.js';
const timer = new Timer();

const USER_DEFINED_TIMER = 5;

interface TodoStore {
	timer: Timer;
	syncStatus: {
		type: 'error' | 'neutral' | null;
		counterMessage?: string;
		statusMessage?: string;
	};
}

// Timer store
export const todoStore = writable<TodoStore>({
	syncStatus: {
		type: null,
		counterMessage: 'Ready',
	},
	timer,
});

export function startTimer() {
	timer.start({
		countdown: true,
		startValues: { seconds: USER_DEFINED_TIMER },
		target: { seconds: 0 },
	});
	todoStore.update((state) => ({
		...state,
		syncStatus: {
			type: 'neutral',
			statusMessage: 'Timer started',
			counterMessage: `Background sync in ${timer.getTimeValues().seconds}s`,
		},
	}));

	// Listen to timer updates and update the sync status
	timer.addEventListener('secondsUpdated', () => {
		todoStore.update((state) => ({
			...state,
			syncStatus: {
				...state.syncStatus,
				counterMessage: `Background sync in ${timer.getTimeValues().seconds}s`,
			},
		}));
	});
}

export function resetTimer() {
	timer.reset();
	todoStore.update((state) => ({
		...state,
		syncStatus: {
			type: 'neutral',
			statusMessage: 'Timer reset',
		},
	}));
}

export function stopTimer() {
	timer.stop();
	todoStore.update((state) => ({
		...state,
		syncStatus: {
			type: 'error',
			statusMessage: 'Timer stopped',
		},
	}));
}

export function pauseTimer() {
	timer.pause();
	todoStore.update((state) => ({
		...state,
		syncStatus: {
			type: 'neutral',
			counterMessage: 'Ready',
			statusMessage: 'Timer paused',
		},
	}));
}
